using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;

using TFX.Core;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.Types;

namespace Projecao.Core.PCR.Integracao
{
    public static class XSQLiteUtils
    {
        public static String GetSQLiteType(String pCSName)
        {
            switch (pCSName)
            {
                case "Decimal":
                    return $"Real";

                case "Int16":
                case "Int32":
                case "Int64":
                case "Boolean":
                    return $"Integer";

                case "Guid":
                case "String":
                case "DateTime":
                    return $"Text";

                case "Byte[]":
                    return $"Blob";

                default:
                    throw new Exception("Tipo \"" + pCSName + "\" não pode ser convertido para SQLite, falta implementação.");
            }
        }

        public static void GetInsert(DbCommand pCommand, XDataSet pTable, XDataTuple pTuple)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb1.Append("INSERT INTO ");
            sb1.Append(pTable.Name);
            sb1.AppendLineEx(" (");
            Int32 num = 0;
            for (int index = 0; index < pTable.Fields.Count; ++index)
            {
                XORMField fld = null;
                fld = pTable.Fields[index];
                if (num > 0)
                {
                    sb1.AppendLineEx(",");
                    sb2.AppendLineEx(",");
                }
                sb1.Append(fld.Alias);
                sb2.Append("@");
                sb2.Append(fld.Alias);
                Object vlr = GetDefaultValue(fld, pTuple[index]);
                AddParam(pCommand, fld, vlr);
                ++num;
            }
            sb1.AppendLineEx(") VALUES (");
            sb1.AppendLineEx(sb2.ToString());
            sb1.AppendLineEx(")");
            pCommand.CommandText = sb1.ToString();
        }

        private static void AddParam(DbCommand pCommand, XORMField pField, Object pValue)
        {
            DbParameter parameter = pCommand.CreateParameter();
            parameter.ParameterName = pField.Alias;
            parameter.DbType = ToDBType(pField.TypeID);
            parameter.Value = CheckValue(pField, pValue);
            if (parameter.Value == null)
                parameter.Value = DBNull.Value;
            pCommand.Parameters.Add(parameter);
        }

        public static Object CheckValue(XORMField pField, Object pValue)
        {
            switch (pField.TypeID.AsString())
            {
                case XGuid.sCID:
                    return ((Guid)pValue).AsString();

                case XDate.sCID:
                case XDateTime.sCID:
                case XTime.sCID:
                    return pValue = ((DateTime)pValue).ToString("yyyy-MM-ddTHH:mm:ss");

                case XInt16.sCID:
                case XInt32.sCID:
                case XInt64.sCID:
                    return pValue ?? 0;

                case XBoolean.sCID:
                    if (pValue is Boolean)
                        return pValue;
                    if (Int32.TryParse(pValue.AsString(), out Int32 bint))
                        return bint == 1;
                    return false;

                default:
                    return pValue;
            }
        }

        public static DbType ToDBType(Guid pTypeID)
        {
            switch (pTypeID.AsString())
            {
                case XBinary.sCID:
                    return DbType.Binary;

                case XBoolean.sCID:
                    return DbType.Int32;

                case XDate.sCID:
                case XTime.sCID:
                case XString.sCID:
                case XDateTime.sCID:
                case XGuid.sCID:
                    return DbType.String;

                case XDecimal.sCID:
                    return DbType.Decimal;

                case XInt16.sCID:
                case XInt64.sCID:
                case XInt32.sCID:
                    return DbType.Int32;

                default:
                    throw new Exception("Tipo \"" + pTypeID + "\" não pode ser convertido para DbType, falta implementação.");
            }
        }

        public static Object GetDefaultValue(XORMField pField, Object pValue)
        {
            if (pField.Type == typeof(Guid) || pValue is Guid)
                return pValue;
            if (pField.Type == typeof(DateTime) && (!(pValue is DateTime) || ((DateTime)pValue < XDefault.NullDateTime)))
                return XDefault.NullDateTime;
            if (pField.Type == typeof(DateTime) || pValue is DateTime)
                return ((DateTime)pValue).ToString("s", CultureInfo.InvariantCulture);
            if (pValue == null)
                pValue = pField.DefaultValue;
            if (pValue is Boolean)
                pValue = ((Boolean)pValue) ? 1 : 0;
            return pValue;
        }

        private static void DefParam(DbCommand pCommand, XORMField pField)
        {
            DbParameter parameter = pCommand.CreateParameter();
            parameter.ParameterName = pField.Alias;
            parameter.DbType = ToDBType(pField.TypeID);
            if (parameter.Value == null)
                parameter.Value = DBNull.Value;
            pCommand.Parameters.Add(parameter);
        }

        public static void PrepareInsert(SQLiteCommand pCommand, XDataSet pTable)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb1.Append("INSERT INTO ");
            sb1.Append(pTable.Name);
            sb1.AppendLineEx(" (");
            Int32 num = 0;
            for (int index = 0; index < pTable.Fields.Count; ++index)
            {
                XORMField fld = null;
                fld = pTable.Fields[index];
                if (num > 0)
                {
                    sb1.AppendLineEx(",");
                    sb2.AppendLineEx(",");
                }
                sb1.Append(fld.Alias);
                sb2.Append("@");
                sb2.Append(fld.Alias);
                DefParam(pCommand, fld);
                ++num;
            }
            sb1.AppendLineEx(") VALUES (");
            sb1.AppendLineEx(sb2.ToString());
            sb1.AppendLineEx(")");
            pCommand.CommandText = sb1.ToString();
        }

        internal static void AddInsertValues(SQLiteCommand pCommand, XDataSet pTable, XDataTuple pTuple)
        {
            for (int index = 0; index < pTable.Fields.Count; ++index)
            {
                XORMField fld = null;
                fld = pTable.Fields[index];
                DbParameter parameter = pCommand.Parameters.FirstOrDefault<DbParameter>(p => p.ParameterName == fld.Alias);
                parameter.Value = GetDefaultValue(fld, pTuple[index]);
            }
        }
    }
}