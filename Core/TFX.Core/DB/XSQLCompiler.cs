using System;
using System.Collections.Generic;
using System.Linq;

using TFX.Core.Exceptions;

namespace TFX.Core.DB
{
	public enum XToRemove
    {
        None = 0
    }
    public enum XOperator : Byte
    {
        EqualTo = 5,
        NotEqualTo = 6,
        GreaterThan = 7,
        GreaterThanOrEqualTo = 8,
        LessThanOrEqualTo = 9,
        LessThan = 10,
        IsNull = 11,
        IsNotNull = 12,
        Like = 13,
        LikeBegin = 14,
        LikeEnd = 15,
        In = 16,
        NotIn = 17,
    }

    public enum XLogic : Byte
    {
        AND = 1,
        OR = 2,
    }

    public enum XParentheses : Byte
    {
        Open = 3,
        Close = 4,
    }
    public abstract class Stmt
    {
    }

    public class SingleOpStmt : Stmt
    {
        public Object Ident;
        public XOperator Op;
    }

    public class ParenthesizedStmt : Stmt
    {
        public Stmt Stmt;
    }

    public class LogicalStmt : Stmt
    {
        public Stmt Left;
        public XLogic Op;
        public Stmt Right;
    }

    public class ListStmt : Stmt
    {
        public Stack<Stmt> Stmts;
    }

    public class BinOpStmt : Stmt
    {
        public Object Ident;
        public XOperator Op;
        public Object Value;
    }

    public class AssignmentStmt : Stmt
    {
        public Object Ident;
        public Object Value;
    }

    public sealed class XParser
    {
        private Stack<Stmt> _Tmp = new Stack<Stmt>();
        private readonly Stmt _Result;
        private Int32 _Index;
        private IList<Object> _Tokens;

        public Stmt Result
        {
            get
            {
                return _Result;
            }
        }

        public XParser(IList<Object> pTokens)
        {
            _Tokens = pTokens;
            _Index = 0;
            while (_Index < _Tokens.Count)
                _Tmp.Push(ParseStmt(_Tmp));
            _Result = (Stmt)new ListStmt()
            {
                Stmts = _Tmp
            };
            if (_Index != _Tokens.Count)
                throw new XError("expected EOF");
        }

        private Boolean IsKeyword(Object pToken)
        {
            return pToken is XOperator || pToken is XLogic;
        }

        private Stmt ParseStmt(Stack<Stmt> pStmtStack)
        {
            Stmt stmt = (Stmt)null;
            if (_Index == _Tokens.Count)
                throw new XError("expected statement, got EOF");
            if ((_Tokens[_Index] is String) && _Index + 1 < _Tokens.Count)
            {
                Object str = _Tokens[_Index];
                if (_Tokens[_Index + 1] is XOperator)
                {
                    XOperator xoperator = (XOperator)_Tokens[_Index + 1];
                    if (xoperator == XOperator.IsNotNull || xoperator == XOperator.IsNull)
                    {
                        SingleOpStmt singleOpStmt = new SingleOpStmt();
                        singleOpStmt.Ident = str;
                        singleOpStmt.Op = xoperator;
                        _Index = _Index + 2;
                        stmt = (Stmt)singleOpStmt;
                    }
                    else
                    {
                        if (_Index + 2 >= _Tokens.Count)
                            throw new XError("Expecting right operand after: " + (Object)xoperator);
                        Object token = _Tokens[_Index + 2];
                        if (IsKeyword(token))
                            throw new XError("Unexpected keyword: " + token);
                        BinOpStmt binOpStmt = new BinOpStmt();
                        binOpStmt.Ident = str;
                        binOpStmt.Op = xoperator;
                        binOpStmt.Value = token;
                        _Index = _Index + 3;
                        stmt = (Stmt)binOpStmt;
                    }
                }
                else
                {
                    if (IsKeyword(_Tokens[_Index + 1]))
                        throw new XError("Unknown token: " + _Tokens[_Index + 1]);
                    AssignmentStmt assignmentStmt = new AssignmentStmt();
                    assignmentStmt.Ident = str;
                    assignmentStmt.Value = _Tokens[_Index + 1];
                    _Index = _Index + 2;
                    stmt = (Stmt)assignmentStmt;
                }
            }
            else if (_Tokens[_Index] is XParentheses && (XParentheses)_Tokens[_Index] == XParentheses.Open)
            {
                _Index = _Index + 1;
                ParenthesizedStmt parenthesizedStmt = new ParenthesizedStmt();
                ListStmt listStmt = new ListStmt();
                listStmt.Stmts = new Stack<Stmt>();
                while (!(_Tokens[_Index] is XParentheses) || (XParentheses)_Tokens[_Index] != XParentheses.Close)
                {
                    if (_Index >= _Tokens.Count)
                        throw new XError(") expected after (");
                    listStmt.Stmts.Push(ParseStmt(listStmt.Stmts));
                }
                parenthesizedStmt.Stmt = (Stmt)listStmt;
                if (_Tokens[_Index] is XParentheses && (XParentheses)_Tokens[_Index] == XParentheses.Close)
                {
                    _Index = _Index + 1;
                    stmt = (Stmt)parenthesizedStmt;
                }
            }
            else
            {
                if (!(_Tokens[_Index] is XLogic))
                    throw new XError("Unknown token: " + _Tokens[_Index]);
                if (pStmtStack.Count == 0)
                    throw new XError("Cannot find left operand for a logic operator");
                LogicalStmt logicalStmt = new LogicalStmt();
                logicalStmt.Left = pStmtStack.Pop();
                logicalStmt.Op = (XLogic)_Tokens[_Index];
                _Index = _Index + 1;
                logicalStmt.Right = ParseStmt(pStmtStack);
                stmt = (Stmt)logicalStmt;
            }
            return stmt;
        }
    }

    public class XSQLCompiler
    {

        public Int32 LastIndex = 1;
        public Boolean UseValue;
        public String Comma;
        public XProvider Provider = XProvider.SQLServer;
        private Dictionary<String, Tuple<Object, Object, XOperator, Object>> _Values = new Dictionary<String, Tuple<Object, Object, XOperator, Object>>();

        public Dictionary<String, Tuple<Object, Object, XOperator, Object>> Ids
        {
            get
            {
                return _Values;
            }
        }

        public static string ParameterSeparator(XProvider pProvider)
        {
                switch (pProvider)
                {
                    case XProvider.SQLServer:
                        return "@";

                    case XProvider.PostgreSQL:
                    case XProvider.Oracle:
                        return ":";

                    default:
                        return "";
                }
            }
        

        public String FeedWhere(Stmt pStmt)
        {
            String str = "";
            if (pStmt is ListStmt)
            {
                IEnumerable<Stmt> source = ((ListStmt)pStmt).Stmts.Reverse<Stmt>();
                Int32 num = 0;
                foreach (Stmt stmt2 in source)
                {
                    str = str + FeedWhere(stmt2);
                    if (num++ < (source.Count<Stmt>() - 1))
                        str = str + " AND ";
                }
                return str;
            }
            if (pStmt is ParenthesizedStmt)
            {
                return str + "(" + FeedWhere(((ParenthesizedStmt)pStmt).Stmt) + ")";
            }
            if (pStmt is LogicalStmt)
            {
                LogicalStmt stmt3 = (LogicalStmt)pStmt;
                String[] textArray1 = new String[] { str, FeedWhere(stmt3.Left), " ", ToString(stmt3.Op), " ", FeedWhere(stmt3.Right) };
                return String.Concat(textArray1);
            }
            if (pStmt is BinOpStmt)
            {
                BinOpStmt stmt4 = (BinOpStmt)pStmt;
                Object ident = stmt4.Ident;
                Object obj2 = stmt4.Value;
                if ((stmt4.Op == XOperator.In) || (stmt4.Op == XOperator.NotIn))
                {
                    if (obj2 is String)
                        return str + IdentValue(ident) + " " + stmt4.Op + " " + obj2;
                    String str3 = ToString(stmt4.Op);

                    String newValue = AddNewValue(ident, obj2, stmt4.Op);
                    if (Provider == XProvider.PostgreSQL)
                        return str + IdentValue(ident) + " = ANY(" + newValue + ")";
                    else
                    if (Provider == XProvider.Oracle)
                        return str + IdentValue(ident) + " in (select to_number(column_value) from xmltable(" + newValue + "))";
                    else
                        return str + IdentValue(ident) + " " + str3.Replace("<@table@>", newValue);
                }

                String[] textArray2 = new String[] { str, IdentValue(ident), " ", ToString(stmt4.Op), " ", AddNewValue(ident, obj2, stmt4.Op) };
                return String.Concat(textArray2);
            }
            if (pStmt is SingleOpStmt)
            {
                SingleOpStmt stmt5 = (SingleOpStmt)pStmt;
                return str + IdentValue(stmt5.Ident) + " " + ToString(stmt5.Op);
            }
            if (pStmt is AssignmentStmt)
            {
                AssignmentStmt stmt6 = (AssignmentStmt)pStmt;
                Object id = stmt6.Ident;
                Object obj3 = stmt6.Value;
                str = str + IdentValue(id) + " = " + AddNewValue(id, obj3, XOperator.EqualTo);
            }
            return str;
        }

        private String IdentValue(Object pIdent)
        {

            String[] vlr = pIdent.ToString().SafeBreak('.');
            String str = String.Join(".", vlr.Select(v => DBObjectName(Provider, v)).ToArray());
            return str;
        }

        public static string DBObjectName(XProvider pProvider, string pField)
        {
            switch (pProvider)
            {
                case XProvider.SQLServer:
                    if (pField.SafeContains("[") || pField.SafeContains("\""))
                        return pField;
                    return pField.AsQuoted();

                case XProvider.PostgreSQL:
                case XProvider.Oracle:
                    if (pField.SafeContains("\""))
                        return pField;
                    return pField.AsQuoted();

                default:
                    return pField;
            }
        }

        private String AddNewValue(Object pID, Object pValue, XOperator pOper)
        {
            if (pValue is String && pOper.In(XOperator.Like, XOperator.LikeEnd, XOperator.LikeBegin))
            {
                String str2 = ((pOper == XOperator.Like) || (pOper == XOperator.LikeEnd)) ? "%" : "";
                String str3 = ((pOper == XOperator.Like) || (pOper == XOperator.LikeBegin)) ? "%" : "";
                pValue = str2 + pValue + str3;
            }
            Int32 lastIndex = LastIndex;
            LastIndex = lastIndex + 1;
            String key = ParameterSeparator(Provider) + "Par" + lastIndex;
            String name = "Par" + lastIndex;
            _Values.Add(key, Tuple.Create<Object, Object, XOperator, Object>(pID, pValue, pOper, name));
            if (UseValue)
                return Comma + pValue + Comma;
            return key;
        }

        private String ToString(XLogic pLogic)
        {
            switch (pLogic)
            {
                case XLogic.AND:
                    return "AND";

                case XLogic.OR:
                    return "OR";

                default:
                    throw new XError("Unknown " + (Object)pLogic);
            }
        }

        private String ToString(XOperator pOper)
        {
            switch (pOper)
            {
                case XOperator.EqualTo:
                    return "=";

                case XOperator.NotEqualTo:
                    return "<>";

                case XOperator.GreaterThan:
                    return ">";

                case XOperator.GreaterThanOrEqualTo:
                    return ">=";

                case XOperator.LessThanOrEqualTo:
                    return "<=";

                case XOperator.LessThan:
                    return "<";

                case XOperator.IsNull:
                    return "is null";

                case XOperator.IsNotNull:
                    return "is not null";

                case XOperator.Like:
                case XOperator.LikeBegin:
                case XOperator.LikeEnd:
                    if (Provider == XProvider.PostgreSQL)
                        return "ilike";
                    return "like";

                case XOperator.In:
                    return "in (select ID from <@table@>)";

                case XOperator.NotIn:
                    return "not in (select ID from <@table@>)";

                default:
                    throw new XError("Unknown " + (Object)pOper);
            }
        }
    }
}
