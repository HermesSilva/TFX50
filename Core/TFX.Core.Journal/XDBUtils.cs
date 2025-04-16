using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using Microsoft.Data.SqlClient;

using TFX.Core;
using TFX.Core.DB;

namespace TFX.Journal
{
    public class XDBUtils : IDisposable
    {
        private string _Host = "localhost";
        private string _DataBase = "APP_TEST_ONLY";
        private string _User;
        private string _Password;
        private SqlConnection _Connection;
        private SqlTransaction _Transaction;

        public XProvider Provider = XProvider.SQLServer;

        public string WhereTag = "<@WHERE@>";
        private string _CnnStr;

        public SqlConnection Connection => _Connection;

        public SqlTransaction Transaction => _Transaction;

        public void SetConnection(string pCnnStr)
        {
            _CnnStr = pCnnStr;
        }

        public void SetConnection(string pUser, string pPassword, string pHost, string pDataBase)
        {
            _Host = pHost;
            _DataBase = pDataBase;
            _User = pUser;
            _Password = pPassword;
        }

        public void ExecuteSQL(String pSQL, params object[] pWhere)
        {
            using (DbCommand cmd = _Connection.CreateCommand())
            {
                cmd.Transaction = _Transaction;
                if (pWhere?.Length > 0)
                    PrepareCommand(cmd, pSQL, pWhere);
                else
                    cmd.CommandText = pSQL;
                cmd.ExecuteNonQuery();
            }
        }

        public DbDataReader ExecuteReader(String pSQL, params object[] pWhere)
        {
            using (DbCommand cmd = _Connection.CreateCommand())
            {
                cmd.Transaction = _Transaction;
                if (pWhere?.Length > 0)
                    PrepareCommand(cmd, pSQL, pWhere);
                else
                    cmd.CommandText = pSQL;
                return cmd.ExecuteReader();
            }
        }

        private void PrepareCommand(DbCommand pCommand, string pSelect, object[] pWhere)
        {
            Dictionary<String, Tuple<Object, Object, XOperator, Object>> isds = GetWhere(pWhere, out String strwhere);
            Tuple<object, object, XOperator, object>[] data = isds.Values.ToArray();
            foreach (KeyValuePair<String, Tuple<Object, Object, XOperator, Object>> id in isds)
            {
                if (pCommand != null)
                {
                    if ((id.Value.Item3 == XOperator.In || id.Value.Item3 == XOperator.NotIn) && id.Value.Item2 is IEnumerable)
                    {
                        throw new InvalidOperationException("Não implementado Enumrados.");
                        //   CreateStructuredParam(id.Key, (IEnumerable)id.Value.Item2, pCommand, ((XORMField)id.Value.Item1).Type);
                    }
                    else
                    {
                        DbParameter parameter = pCommand.CreateParameter();
                        parameter.ParameterName = id.Value.Item4.ToString();
                        parameter.Value = id.Value.Item2 ?? DBNull.Value;
                        pCommand.Parameters.Add(parameter);
                    }
                }
            }
            if (pSelect.SafeContains(WhereTag))
                pCommand.CommandText = pSelect.SafeReplace(WhereTag, " where " + strwhere);
            else
                pCommand.CommandText = pSelect + " where " + strwhere;
        }

        private void CreateStructuredParam(String pParameterName, IEnumerable pIDs, DbCommand pCommand, Type pType)
        {
            DataTable dtbl = new DataTable();
            if (pType == typeof(Guid))
                dtbl.Columns.Add("ID", typeof(Guid));
            else
                dtbl.Columns.Add("ID", typeof(Int64));
            foreach (Object pId in pIDs)
                dtbl.Rows.Add(new Object[] { pId });
            SqlParameter sp = (SqlParameter)pCommand.CreateParameter();
            sp.Value = dtbl;
            sp.ParameterName = pParameterName;
            sp.SqlDbType = SqlDbType.Structured;
            if (pType == typeof(Guid))
                sp.TypeName = "dbo.IDGuids";
            else
                sp.TypeName = "dbo.ID64s";
            pCommand.Parameters.Add(sp);
        }

        private Dictionary<String, Tuple<Object, Object, XOperator, Object>> GetWhere(Object[] pWhere, out String pResult)
        {
            XSQLCompiler sqlcompiler = new XSQLCompiler();
            XParser parser = new XParser(pWhere);
            pResult = sqlcompiler.FeedWhere(parser.Result);
            return sqlcompiler.Ids;
        }

        private string GetConnectionString()
        {
            if (_CnnStr.IsFull())
                return _CnnStr;
            return $"Data Source={_Host};Initial Catalog={_DataBase};Persist Security Info=False;User ID={_User};Password={_Password};Integrated Security={_User.IsEmpty()};MultipleActiveResultSets=True;TrustServerCertificate=true;Connection Timeout=90";
        }

        public void BeginTransaction()
        {
            _Transaction = _Connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _Transaction.Commit();
            }
            catch (Exception ex)
            {
            }
        }

        public void Rollback()
        {
            try
            {
                _Transaction?.Rollback();
            }
            catch (Exception ex)
            {
            }
        }

        public void Open()
        {
            _Connection = new SqlConnection(GetConnectionString());
            _Connection.Open();
        }

        public int Insert(string pTableName, List<string> pColumns, object[] pValues)
        {
            StringBuilder sb = new StringBuilder();
            using (DbCommand cmd = _Connection.CreateCommand())
            {
                cmd.Transaction = _Transaction;
                for (int i = 0; i < pColumns.Count; i++)
                {
                    string col = pColumns[i];
                    sb.Append(XSQLCompiler.ParameterSeparator(Provider) + col);
                    if (i < pColumns.Count - 1)
                        sb.Append(',');
                    if (pValues[i] is DateTime && ((DateTime)pValues[i]) < new DateTime(1756, 1, 1, 1, 1, 1))
                        pValues[i] = new DateTime(1756, 1, 1, 0, 0, 0);
                    DbParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = col;
                    parameter.Value = pValues[i] ?? DBNull.Value;
                    cmd.Parameters.Add(parameter);
                }
                cmd.CommandText = $"insert into {XSQLCompiler.DBObjectName(Provider, pTableName)} ({string.Join(",", pColumns)}) values ({sb})";
                return cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            try
            {
                _Transaction?.Rollback();
            }
            catch
            {
            }
            _Connection?.Dispose();
        }
    }
}
