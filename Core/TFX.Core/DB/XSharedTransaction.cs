using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Data;
using System.Data.Common;

namespace TFX.Core
{
    public class XCommandInterceptor : DbCommandInterceptor
    {
        private IHttpContextAccessor _HttpContextAccessor;
        private XISharedTransaction _SharedTransaction;

        public XCommandInterceptor(IHttpContextAccessor httpContextAccessor, XISharedTransaction pSharedTransaction = null)
        {
            _HttpContextAccessor = httpContextAccessor;
            _SharedTransaction = pSharedTransaction;
        }

        public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        {
            SetContext(eventData);
            var cmd = base.CommandCreated(eventData, result);
            if (_SharedTransaction != null)
                cmd.Transaction = _SharedTransaction.GetTransaction();
            return cmd;
        }

        private void SetContext(CommandCorrelatedEventData eventData)
        {
            if (_HttpContextAccessor == null)
                return;
            try
            {
         //       var user = _HttpContextAccessor.HttpContext.GetUser();
         //       long sequencia = DateTime.UtcNow.Ticks - new DateTime(2024, 05, 20, 10, 26, 13).Ticks;
         //       string revisao = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "");
         //       string data = $@"exec sys.sp_set_session_context @key = 'tranid', @value = '{revisao}';
								 //exec sys.sp_set_session_context @key = 'idsequencia', @value = '{sequencia}';
								 //exec sys.sp_set_session_context @key = 'idusuario', @value = '{user.UsuarioID}';
								 //exec sys.sp_set_session_context @key = 'idescritorio', @value = '{user.Escritorio}';
								 //exec sys.sp_set_session_context @key = 'idempresa', @value = '{user.Empresa}';";

         //       using (DbCommand cmd = eventData.Connection.CreateCommand())
         //       {
         //           if (_SharedTransaction != null)
         //               cmd.Transaction = _SharedTransaction.GetTransaction();
         //           else
         //               cmd.Transaction = eventData.Context.Database.CurrentTransaction?.GetDbTransaction();
         //           cmd.CommandText = data;
         //           cmd.ExecuteNonQuery();
         //       }
            }
            catch
            {
            }
        }
    }

    public interface XISharedTransaction : IDisposable
    {
        DbTransaction GetTransaction();
        SqlConnection GetConnection(string pConnectionString);
        void Commit();
        void Rollback();
    }

    public sealed class XSharedTransaction : XISharedTransaction
    {
        private SqlConnection _Connection;
        private DbTransaction _Transaction;

        public SqlConnection GetConnection(string pConnectionString)
        {
            if (_Connection == null)
            {
                _Connection = new SqlConnection(pConnectionString);
                _Connection.Open();
                _Transaction = _Connection.BeginTransaction();
            }
            return _Connection;
        }

        public DbTransaction GetTransaction()
        {
            return _Transaction;
        }

        public void Commit()
        {
            if (_Transaction.Connection != null)
                _Transaction.Commit();
            _Connection.Close();
        }

        public void Rollback()
        {
            if (_Transaction.Connection != null)
                _Transaction.Rollback();
            if (_Connection.State == ConnectionState.Closed)
                _Connection.Close();
        }

        public void Dispose()
        {
            _Transaction?.Dispose();
            _Connection?.Dispose();
        }
    }
}
