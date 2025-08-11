using System;
using System.Collections.Generic;
using System.Linq;

using TFX.Core;
using TFX.Core.Collections;
using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.Types;
using TFX.Core.Objects;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Utils;

namespace Projecao.Core.LGC.DB
{
    public class XOracleLegacyDBReverse : XClass
    {
        public readonly XHashSet<XORMLegacyTable, Guid> Tables = new XHashSet<XORMLegacyTable, Guid>();

        public XOracleLegacyDBReverse(XExecContext pContext)
        {
            SetContext(pContext);
        }

        protected XExecContext Context;
        protected XDBFactory Factory;

        public void SetContext(XExecContext pContext)
        {
            Context = pContext;
            Factory = pContext.DataBase.Factory;
        }

        public virtual void Execute(String[] pTables)
        {
            ReverseTables(pTables);
        }

        public void ReverseTables(String[] pTables)
        {
            String str1 = TableReverseSQL;
            using (XQuery pQuery = new XQuery(Context))
            {
                pQuery.SQL = str1.SafeReplace("<@WHERE@>", " and cl.table_name in ('" + String.Join("','", pTables) + "')");
                if (!pQuery.Open())
                    return;
                String str2 = "";
                XORMLegacyTable tbl = null;
                List<XORMPKField> pk = new List<XORMPKField>();
                foreach (XDataTuple tpl in pQuery)
                {
                    if (str2 != tpl["table_name"].As<String>())
                    {
                        if (tbl != null)
                        {
                            tbl.SetPK(pk.ToArray());
                            if (pk.Count == 0)
                                XConsole.Error($"Table sem PK[{tbl.Name}]");
                        }
                        pk.Clear();
                        tbl = new XORMLegacyTable();
                        tbl.Name = tpl["table_name"].As<String>();
                        tbl.ID = Guid.NewGuid();
                        str2 = tbl.Name;
                        Tables.Add(tbl, tbl.ID);
                    }
                    if (tpl["primary_key"].ToString() == "1")
                    {
                        XORMPKField field = new XORMPKField();
                        GetField(tpl, field);
                        field.Owner = tbl;
                        tbl.Fields.Add(field);
                        pk.Add(field);
                    }
                    else
                    {
                        XORMField field = new XORMField();
                        GetField(tpl, field);
                        field.Owner = tbl;
                        tbl.Fields.Add(field);
                    }
                }
                if (tbl != null)
                {
                    tbl.SetPK(pk.ToArray());
                    if (pk.Count == 0)
                        XConsole.Error($"Table sem PK[{tbl.Name}]");
                }
            }
        }

        private XORMField GetField(XDataTuple pTuple, XORMField pField)
        {
            String dbtype = pTuple["type_name"].As<String>().ToLower();
            pField.TypeID = Factory.GetCSType(dbtype);
            pField.IsLegacy = true;
            if (pField.TypeID.IsEmpty())
            {
                Console.WriteLine($"Campo=[{pTuple["column_name"].As<String>()} Types=[{dbtype}]");
                return pField;
            }
            if (pField.XType == null)
                throw new XError($"Não foi encontrado XType para [{dbtype}].");
            pField.Name = pTuple["column_name"].As<String>();
            pField.LegacyType = pTuple["orininal_type"].As<String>();
            SetMaxLenth(pTuple, pField, dbtype);
            pField.Scale = pTuple["scale"].As<Int32>();
            pField.IsNullable = Convert.ToInt32(pTuple["is_nullable"]) == 1;
            if (pTuple.DataSet.Fields.Any(f => f.Name == "defname"))
                pField.DefName = pTuple["defname"].As<String>();
            pField.ID = Guid.NewGuid();
            if (!XUtils.In<Guid>(pField.TypeID, new Guid[3] { XString.CID, XBinary.CID, XDecimal.CID }))
            {
                pField.Length = 0;
                pField.Scale = 0;
            }
            return pField;
        }

        protected void SetMaxLenth(XDataTuple pTuple, XORMField pField, String pDBType)
        {
            if (pDBType.SafeLower() == "nvarchar")
                pField.Length = Math.Max(0, pTuple["max_length"].As<Int32>() / 2);
            else
                pField.Length = Math.Max(0, Math.Max(pTuple["precision"].As<Int32>(), pTuple["max_length"].As<Int32>()));
        }

        protected String TableReverseSQL
        {
            get
            {
                return $"SELECT                                                                 \r\n" +
                       $"    CAST(cl.column_id AS NUMBER(8)) \"column_id\",                     \r\n" +
                       $"    CAST(cl.table_name AS NVARCHAR2(128)) \"table_name\",              \r\n" +
                       $"    CAST(cl.column_name AS NVARCHAR2(128)) \"column_name\",            \r\n" +
                       $"    CAST(COALESCE(cl.char_col_decl_length,                             \r\n" +
                       $"    0) AS NUMBER(8)) \"max_length\",                                   \r\n" +
                       $"    CAST(COALESCE(cl.data_precision,                                   \r\n" +
                       $"    0) AS NUMBER(8)) \"precision\",                                    \r\n" +
                       $"    cl.data_type \"orininal_type\",                                    \r\n" +
                       $"    CAST(cl.data_length AS NUMBER(8)) \"scale\",                       \r\n" +
                       $"    CASE                                                               \r\n" +
                       $"        WHEN cl.nullable = 'N' THEN 0                                  \r\n" +
                       $"        ELSE 1                                                         \r\n" +
                       $"    END \"is_nullable\",                                               \r\n" +
                       $"    CAST(CASE                                                          \r\n" +
                       $"          WHEN((cl.data_type = 'NUMBER'                                \r\n" +
                       $"                AND cl.data_precision > 19)                            \r\n" +
                       $"               OR cl.data_scale > 0) THEN 'decimal'                    \r\n" +
                       $"          WHEN(cl.data_type = 'NUMBER'                                 \r\n" +
                       $"               AND cl.data_precision = 1                               \r\n" +
                       $"               AND cl.data_length = 22) THEN 'integer'                 \r\n" +
                       $"          WHEN(cl.data_type = 'NUMBER'                                 \r\n" +
                       $"               AND cl.data_precision = 19                              \r\n" +
                       $"               AND cl.data_length = 22) THEN 'bigint'                  \r\n" +
                       $"          WHEN((cl.data_type = 'NUMBER') and ((cl.data_precision       \r\n" +
                       $"               is null) or (cl.data_precision between 1 and 10))       \r\n" +
                       $"               and (cl.data_scale = 0 or cl.data_scale is null)        \r\n" +
                       $"               and (cl.data_length = 22)) then 'integer'               \r\n" +
                       $"          WHEN(cl.data_type = 'NUMBER'                                 \r\n" +
                       $"               AND cl.data_precision = 5                               \r\n" +
                       $"               AND cl.data_length = 22) THEN 'smallint'                \r\n" +
                       $"          WHEN cl.data_type = 'TIMESTAMP(6)' THEN 'timestamp'          \r\n" +
                       $"        ELSE                                                           \r\n" +
                       $"              cl.data_type                                             \r\n" +
                       $"        END AS NVARCHAR2(128)) \"type_name\",                          \r\n" +
                       $"    COALESCE(PK.ISPK,0) \"primary_key\"                                \r\n" +
                       $"FROM                                                                   \r\n" +
                       $"    user_tab_columns cl                                                \r\n" +
                       $"INNER JOIN user_tables tb ON                                           \r\n" +
                       $"    tb.table_name = cl.table_name                                      \r\n" +
                       $"LEFT JOIN (SELECT 1 ISPK,cols.position column_position,                \r\n" +
                       $"                          cols.table_name, cols.column_name,           \r\n" +
                       $"           cols.position, cons.status, cons.owner FROM                 \r\n" +
                       $"           user_constraints cons, user_cons_columns cols WHERE         \r\n" +
                       $"            cons.constraint_type = 'P' AND cons.constraint_name =      \r\n" +
                       $"            cols.constraint_name ) PK ON                               \r\n" +
                       $"    PK. column_name = cl.column_name                                   \r\n" +
                       $"    AND PK.Table_name = cl.Table_name                                  \r\n" +
                       $"WHERE                                                                  \r\n" +
                       $"        tb.table_name NOT LIKE '%$%'   <@WHERE@>                       \r\n" +
                       $"ORDER BY                                                               \r\n" +
                       $"    cl.table_name,                                                     \r\n" +
                       $"    column_position,                                                   \r\n" +
                       $"    cl.column_name                                                     \r\n";
            }
        }
    }
}