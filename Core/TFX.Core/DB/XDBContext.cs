using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Exceptions;
using TFX.Core.Interfaces;
using TFX.Core.Identity;
using TFX.Core.Data;
using TFX.Core.IDs.Model;

namespace TFX.Core
{

    public abstract class XEntity : XIReflectable
    {
        public static StringBuilder GetRules(params Type[] pType)
        {
            var flds = new List<string>();
            var sb = new StringBuilder();
            foreach (var tp in pType.OrderBy(t => t.Name))
            {

                var ppts = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default);
                foreach (var ppt in ppts)
                {
                    var rq = ppt.GetCustomAttribute<RequiredAttribute>();
                    var disp = ppt.GetCustomAttribute<DisplayAttribute>()?.Name ?? ppt.Name;
                    var len = ppt.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                    if (rq != null || len > 0)
                        flds.Add($"O campo <b>{disp.AsQuoted()}</b> {(rq != null ? " é de preenchimento obrigatório" : "")}" +
                            $"{(len > 0 ? $" e possui limite máximo de <b>{len}</b> {(len == 1 ? "carácter" : "caracteres")}" : "")}.");
                }
            }
            if (flds.Count > 0)
            {
                sb.AppendLine("<b>Regras para preenchimento dos campos</b>");
                sb.Append(string.Join(Environment.NewLine, flds));
            }
            return sb;
        }

        public void Validate(StringBuilder pBuilder)
        {
            
            foreach (var ppt in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default))
            {
                var rq = ppt.GetCustomAttribute<RequiredAttribute>();
                var disp = ppt.GetCustomAttribute<DisplayAttribute>()?.Name ?? ppt.Name;
                var len = ppt.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                var vlr = ppt.GetValue(this);
                if (rq != null)
                {
                    if (ppt.PropertyType == typeof(String) && vlr.AsString().IsEmpty())
                        pBuilder.AppendLine($"Erro: O campo {disp.AsQuoted()} não pode ser nulo.");
                }
                if (len > 0 && vlr.AsString().SafeLength() > len)
                    pBuilder.AppendLine($"Erro: O tamanho do conteudo do campo {disp.AsQuoted()} não pode ser maior que {len} caractéres.");
            }
        }
    }

    public class XTenantEntity : XEntity
    {
        [Display(Name = "CPF/CNPJ")]
        [MaxLength(14)]
        [Required()]
        public String CPFCNPJ
        {
            get; set;
        }
    }

    public delegate void XOnModelCreating(ModelBuilder pBuilder);

    public enum XProvider
    {
        None,
        SQLServer,
        PostgreSQL,
        Oracle,
    }

    public class ConnectionMonitorInterceptor : DbConnectionInterceptor
    {
        private static int _ActiveConnections = 0;

        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            Interlocked.Increment(ref _ActiveConnections);
            base.ConnectionOpened(connection, eventData);
            XConsole.Debug($"Active connetion {_ActiveConnections} ++");
        }

        public override void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
        {
            Interlocked.Decrement(ref _ActiveConnections);
            base.ConnectionClosed(connection, eventData);
            XConsole.Debug($"Active connetion {_ActiveConnections} --");
        }

        public static int GetActiveConnections() => _ActiveConnections;
    }

    public class XDBContext : DbContext, XIUseContext
    {
        public XDBContext(DbContextOptions pOptions, XITenantProvider pTenantProvider, XISharedTransaction pSharedTransaction)
                : base(pOptions)
        {
            SharedTransaction = pSharedTransaction;
            TenantProvider = pTenantProvider;
            PrepareConnection();
        }

        public XDBContext(DbContextOptions pOptions, XITenantProvider pTenantProvider)
                : base(pOptions)
        {
            TenantProvider = pTenantProvider;
            PrepareConnection();
        }


        public XDBContext(DbContextOptions pOptions)
               : base(pOptions)
        {
            PrepareConnection();
        }

        public Boolean ByPassTenant
        {
            get; set;
        }

        protected List<XOnModelCreating> ToExecute = new List<XOnModelCreating>();
        private string _Host;
        private string _DataBase;
        private string _User;
        private string _Password;
        protected string ConnectionString;
        private bool _Disposed;
        private bool _HasTransaction;
        public readonly XISharedTransaction SharedTransaction;
        public readonly XITenantProvider TenantProvider;
        private List<Type> _RemoveFromTenant = new List<Type>();
        private CancellationToken _CancellationToken;
        private XUserSession _Session;

        protected void RemoveFromTenant<T>()
        {
            var tp = typeof(T);
            if (_RemoveFromTenant.Contains(tp))
                return;
            _RemoveFromTenant.Add(tp);
        }

        public void SetConnection(string pConnectionString)
        {
            ConnectionString = pConnectionString;
        }

        public void SetConnection(string pHost, string pDataBase, string pUser = null, string pPassword = null)
        {
            _Host = pHost;
            _DataBase = pDataBase;
            _User = pUser;
            _Password = pPassword;
        }
        public override int SaveChanges()
        {
            if (SharedTransaction != null)
                Database.UseTransaction(SharedTransaction.GetTransaction());
            return base.SaveChanges();
        }

        public void BeginTransaction()
        {
            SharedTransaction?.GetConnection(GetConnectionString());
        }

        public void Rollback()
        {
            try
            {
                SharedTransaction?.Rollback();
            }
            catch
            {
            }
        }

        public void Commit()
        {
            SharedTransaction?.Commit();
        }

        private void PrepareConnection()
        {
            var provider = XEnvironment.Read("DB_PROVIDER", "SQLServer");
            if (provider.IsEmpty())
                throw new XError("Value for evironment variable 'DB_PROVIDER' is not found");
            Provider = Enum.Parse<XProvider>(provider);
            if (SharedTransaction != null)
                SharedTransaction.GetConnection(GetConnectionString());
        }

        protected virtual void CreateProcedures(DatabaseFacade pDataBase)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder pBuilder)
        {
            pBuilder.AddInterceptors(new XCommandInterceptor(null, SharedTransaction));
            SelectProvider(pBuilder);

#if DEBUG

            pBuilder.AddInterceptors(new ConnectionMonitorInterceptor());
            pBuilder.EnableSensitiveDataLogging(true);
            pBuilder.LogTo(s => Debug.WriteLine(s));
            pBuilder.EnableDetailedErrors(true);
            pBuilder.EnableSensitiveDataLogging(true);
#endif
            base.OnConfiguring(pBuilder);
        }

        private void SelectProvider(DbContextOptionsBuilder pBuilder)
        {
            switch (Provider)
            {
                case XProvider.SQLServer:
                    if (SharedTransaction == null)
                        pBuilder.UseSqlServer(GetConnectionString());
                    else
                        pBuilder.UseSqlServer(SharedTransaction.GetConnection(GetConnectionString()));
                    break;

                //case XProvider.PostgreSQL:
                //    pBuilder.UseNpgsql(GetConnectionString());
                //    //.ReplaceService<ISqlGenerationHelper, NpgsqlSqlGenerationLowercasingHelper>();
                //    break;

                //case XProvider.Oracle:
                //    pBuilder.UseOracle(GetConnectionString());
                //    break;

                default:
                case XProvider.None:
                    throw new XError("the current provider is not known.");
            }
        }

        protected String DelimitIdentifier(String pIdentifier)
        {
            switch (Provider)
            {
                case XProvider.SQLServer:
                    return $"\"{pIdentifier}\"";

                case XProvider.PostgreSQL:
                    return $"\"{pIdentifier}\"";

                case XProvider.Oracle:
                    return $"\"{pIdentifier}\"";

                default:
                case XProvider.None:
                    throw new XError("the current provider is not known.");
            }
        }
        public String GetConnection()
        {
            return GetConnectionString();
        }

        protected virtual String GetConnectionString()
        {
            if (ConnectionString != null)
                return ConnectionString;
            return XEnvironment.Read("SQL_SERVER_TFX", "");
        }        

        public XProvider Provider
        {
            get;
            private set;
        }

        public String GetDBType(String pCSType, Int32 pLength = 0, Int32 pScale = 0)
        {
            switch (Provider)
            {
                case XProvider.SQLServer:
                    return GetSQLServerDBType(pCSType, pLength, pScale);

                case XProvider.PostgreSQL:
                    return GetPostgreSQLDBType(pCSType, pLength, pScale);

                case XProvider.Oracle:
                    return GetOracleDBType(pCSType, pLength, pScale);

                default:
                case XProvider.None:
                    throw new XError("the current provider is not known.");
            }
        }

        private string GetPostgreSQLDBType(string pCSType, int pLength, int pScale)
        {
            switch (pCSType)
            {
                case "DateTime":
                    return "timestamp";

                case "Byte[]":
                    return "bytea";

                case "Boolean":
                    return "boolean";

                case "Decimal":
                    return $"numeric({pLength},{pScale})";

                case "Guid":
                    return "UUID";

                case "Int16":
                    return "smallint";

                case "Int32":
                    return "integer";

                case "Int64":
                    return "bigint";

                case "String":
                    if (pLength > 0)
                        return $"varchar({pLength})";
                    else
                        return "text";

                default:
                    throw new Exception($"Não implemento conversão para tipo [{pCSType}].");
            }
        }

        public override ValueTask DisposeAsync()
        {
            _Disposed = true;
            return base.DisposeAsync();
        }

        public override void Dispose()
        {
            _Disposed = true;
            Rollback();
            base.Dispose();
        }

        private string GetOracleDBType(string pCSType, int pLength, int pScale)
        {
            switch (pCSType)
            {
                case "DateTime":
                    return "timestamp";

                case "Byte[]":
                    return "bytea";

                case "Boolean":
                    return "boolean";

                case "Decimal":
                    return $"numeric({pLength},{pScale})";

                case "Guid":
                    return "UUID";

                case "Int16":
                    return "smallint";

                case "Int32":
                    return "integer";

                case "Int64":
                    return "bigint";

                case "String":
                    if (pLength > 0)
                        return $"varchar({pLength})";
                    else
                        return "text";

                default:
                    throw new Exception($"Não implemento conversão para tipo [{pCSType}].");
            }
        }

        private static string GetSQLServerDBType(string pCSType, int pLength, int pScale)
        {
            switch (pCSType)
            {
                case "DateTime":
                    return "datetime2";

                case "Byte[]":
                    if (pLength == 0)
                        return "varbinary(max)";
                    else
                        return $"varbinary({pLength})";

                case "Boolean":
                    return "bit";

                case "Decimal":
                    return $"numeric({pLength},{pScale})";

                case "Guid":
                    return "uniqueidentifier";

                case "Int16":
                    return "smallint";

                case "Int32":
                    return "int";

                case "Int64":
                    return "bigint";

                case "Double":
                    return "float";

                case "DateTimeOffset":
                    return "DateTimeOffset";

                case "String":
                    if (pLength > 0)
                        return $"varchar({pLength})";
                    else
                        return "varchar(max)";

                default:
                    throw new Exception($"{nameof(GetSQLServerDBType)}, Não implemento conversão para tipo [{pCSType}].");
            }
        }

        public String GetDBValue(String pCSType, Object pValue)
        {
            switch (Provider)
            {
                case XProvider.SQLServer:
                    return GetMSSQlServerDBValue(pCSType, pValue);

                case XProvider.PostgreSQL:
                    return GetPostgreSQDBValue(pCSType, pValue);

                case XProvider.Oracle:
                    return GetOracleDBValue(pCSType, pValue);

                default:
                case XProvider.None:
                    throw new XError("the current provider is not known.");
            }
        }

        private String GetOracleDBValue(string pCSType, object pValue)
        {
            switch (pCSType)
            {
                case "DateTime":
                case "Guid":
                case "String":
                    return $"{pValue?.ToString()}";

                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Boolean":
                    return pValue.ToString();

                default:
                    throw new Exception($"Não implemento conversão para tipo [{pCSType}].");
            }
        }

        private String GetPostgreSQDBValue(string pCSType, object pValue)
        {
            switch (pCSType)
            {
                case "DateTime":
                case "Guid":
                case "String":
                    return $"{pValue?.ToString()}";

                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Boolean":
                    return pValue.ToString();

                default:
                    throw new Exception($"Não implemento conversão para tipo [{pCSType}].");
            }
        }

        private static string GetMSSQlServerDBValue(string pCSType, object pValue)
        {
            switch (pCSType)
            {
                case "DateTime":
                case "Guid":
                case "String":
                    return $"{pValue?.ToString()}";

                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Boolean":
                    return pValue.ToString();

                default:
                    throw new Exception($"{nameof(GetMSSQlServerDBValue)} , Não implemento conversão para tipo [{pCSType}].");
            }
        }


        public static IConfiguration Configuration
        {
            get;
            set;
        }

        public static IServiceCollection Services
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder pBuilder)
        {
            PrepareTenanFilter(pBuilder);
        }

        protected virtual void PrepareTenanFilter(ModelBuilder pBuilder)
        {
            foreach (var entityType in pBuilder.Model.GetEntityTypes())
            {
                if (_RemoveFromTenant.Contains(entityType.ClrType))
                    continue;
                if (typeof(XTenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = GetType().GetMethod(nameof(ApplyTenantFilter), BindingFlags.Public | BindingFlags.Instance);
                    if (method == null)
                        return;
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(this, [pBuilder]);
                }
            }
        }

        public void ApplyTenantFilter<TEntity>(ModelBuilder pBuilder) where TEntity : XTenantEntity
        {
            pBuilder.Entity<TEntity>().HasQueryFilter(e => GetTennatID() == null || e.CPFCNPJ == GetTennatID());
        }

        private string GetTennatID()
        {
            return TenantProvider.GetTenantID(ByPassTenant);
        }

        protected XUserSession Session => _Session;
        protected CancellationToken CancellationToken => _CancellationToken;

        public virtual void SetContextData(CancellationToken pCancellationToken, XUserSession pSession)
        {
            _CancellationToken = pCancellationToken;
            _Session = pSession;
            this.InternalSetContext(pCancellationToken, pSession);
            CancellationToken.Register(OnCancel);
        }

        protected virtual void OnCancel()
        {
        }
    }
}
