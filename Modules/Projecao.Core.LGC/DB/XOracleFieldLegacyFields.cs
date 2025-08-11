using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.Types;
using TFX.Core.Service.DB;
using TFX.Core.Utils;

namespace Projecao.Core.LGC.DB
{
    public static class XOracleFieldLegacyFields
    {
        public static String[] DBFunctions = new String[4] { "getdate", "sysdatetime", "newid", "newsequentialid" };

        public static String GetCreate(XORMField pField, Boolean pAddDefault, Boolean pForceDefault, XDBFactory pFactory, Boolean pUseCheck = true)
        {
            return pFactory.DBObjectName(pField.Name) + " " + GetDBType(pFactory, pField) + " " + Extends(pField, pAddDefault, pForceDefault, pFactory, pUseCheck);
        }

        public static String GetDBType(XDBFactory pFactory, XORMField pField)
        {
            switch (pField.TypeID.AsString())
            {
                case XDecimal.sCID:
                    return $"number({Math.Max(pField.Length, 16)},{pField.Scale})";

                case XString.sCID:
                    if (pField.Length > 0)
                        return $"varchar2({pField.Length})";
                    else
                        return "clob";

                default:
                    return pFactory.GetDBType(pField.TypeID);
            }
        }

        private static String Extends(XORMField pField, Boolean pAddDefault, Boolean pForceDefault, XDBFactory pFactory, Boolean pUseCheck = true)
        {
            String str = pField.IsNullable ? "NULL " : "NOT NULL ";
            if (pAddDefault)
            {
                String pValue = GetDefault(pField);
                if (pForceDefault && pValue.IsEmpty() && !pField.IsNullable)
                    pValue = ForceDefault(pField, pFactory);
                str = pValue + " " + str;
            }
            String mi = "", ma = "";
            switch (pField.Type.Name)
            {
                case "Byte":
                    mi = "0";
                    ma = "255";
                    break;

                case "Int16":
                    mi = Int16.MinValue.AsString();
                    ma = Int16.MaxValue.AsString();
                    break;

                case "Int32":
                    mi = Int32.MinValue.AsString();
                    ma = Int32.MaxValue.AsString();
                    break;

                case "Int64":
                    mi = Int64.MinValue.AsString();
                    ma = Int64.MaxValue.AsString();
                    break;

                case "Boolean":
                    mi = "0";
                    ma = "1";
                    break;
            }
            if (ma.IsFull() && pUseCheck)
                str += $"constraint CHK_{pField.ID} check({pFactory.DBObjectName(pField.Name)} between {mi} and {ma})";
            return str;
        }

        private static String ForceDefault(XORMField pField, XDBFactory pFactory)
        {
            XType type = XType.Get(pField.TypeID);

            switch (type.Type.Name)
            {
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Numeric":
                case "Boolean":
                    return "default 0";

                case "Time":
                case "Date":
                case "DateTime":
                    return "default sysdatetime()";

                case "Guid":
                    return "default '00000000-0000-0000-0000-000000000000'";

                case "Byte[]":
                    return "default 0x00";

                case "String":
                    return "default ''";

                default:

                    throw new XError("Tipo não implementado para [" + type.Type.Name + "].");
            }
        }

        public static String GetDefault(XORMField pField)
        {
            if (pField.DefaultValue.AsString((String)null).IsEmpty())
                return "";
            String vlr = pField.DefaultValue.AsString((String)null);
            if (DBFunctions.Any(f => f == vlr.ToLower()))
                vlr += "()";
            else
            if (XUtils.In(pField.TypeID, XString.CID, XGuid.CID))
                vlr = "'" + vlr + "'";
            if (pField.TypeID == XBoolean.CID)
                vlr = "'" + vlr.AsString().ToLower() + "'";
            if ("getdate();sysdatetime();sysdatetimeoffset()".Contains(vlr.ToLower()))
                vlr = "current_timestamp";

            if (vlr.SafeLower() == "'true'")
                vlr = "1";
            if (vlr.SafeLower() == "'false'")
                vlr = "0";
            vlr = "(" + vlr + ")";
            return " default " + vlr;
        }

        public static XScript GetAlter(XORMField pField, XORMField pOldField, XDBFactory pFactory)
        {
            XScript scr = new XScript(pField);
            scr.Type = XObjectType.Field;
            scr.Action = XScriptAction.Alter;
            String nl = "";
            if (pOldField.IsNullable != pField.IsNullable)
                nl = Extends(pField, false, false, pFactory);

            scr.Script = $"alter table {pFactory.DBObjectName(pOldField.Owner.Name)} modify({pFactory.DBObjectName(pField.Name)} {pFactory.GetDBType(pField)} {nl})";
            if (pField.DefName.IsFull())
                scr.Before.Add(GetDropDefault(pField, pFactory));
            return scr;
        }

        private static XScript GetDropDefault(XORMField pField, XDBFactory pFactory)
        {
            XScript scr = new XScript(pField);
            scr.Type = XObjectType.Default;
            scr.Action = XScriptAction.Drop;
            scr.Script = $"ALTER TABLE {pFactory.DBObjectName(pField.Owner.Name)} DROP CONSTRAINT {pFactory.DBObjectName(pField.DefName)}";
            return scr;
        }

        public static XScript GetAdd(XORMField pField, XDBFactory pFactory)
        {
            XScript scr = new XScript(pField.Owner);
            scr.Action = XScriptAction.Create;
            scr.Type = XObjectType.Field;
            scr.Script = "alter table " + pFactory.DBObjectName(pField.Owner.Name) + " add " + GetCreate(pField, true, true, pFactory);
            return scr;
        }

        public static XScript GetDrop(XORMTable pTable, XORMField pField, XDBFactory pFactory)
        {
            XScript scr = new XScript(pField);
            scr.Action = XScriptAction.Drop;
            scr.Type = XObjectType.Field;
            scr.Script = "alter table " + pFactory.DBObjectName(pTable.Name) + " drop column " + pFactory.DBObjectName(pField.Name);
            if (pField.DefName.IsFull())
                scr.Before.Add(GetDropDefault(pField, pFactory));
            return scr;
        }

        internal static XScript GetRename(XORMField pField, XORMField pOldFiled, XDBFactory pFactory)
        {
            XScript scr = new XScript(pField);
            scr.Action = XScriptAction.Rename;
            scr.Type = XObjectType.Field;
            scr.Script = $"alter table {pFactory.DBObjectName(pOldFiled.Owner.Name)} rename column {pFactory.DBObjectName(pOldFiled.Name)} to {pFactory.DBObjectName(pField.Name)}";
            return scr;
        }
    }
}