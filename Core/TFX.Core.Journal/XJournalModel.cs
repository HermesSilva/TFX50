using System;
using System.Collections.Generic;
using System.Linq;

namespace TFX.Journal
{
    public partial class Selects
    {
        public int idSelects
        {
            get; set;
        }

        public int idTabelas
        {
            get; set;
        }

        public string Nome
        {
            get; set;
        }

        public string Query
        {
            get; set;
        }

        public string QueryDado
        {
            get; set;
        }
    }

    public partial class Tabelas
    {
        public int idTabelas
        {
            get; set;
        }

        public string Nome
        {
            get; set;
        }

        public string Esquema
        {
            get; set;
        }

        public string CampoPK
        {
            get; set;
        }

        public bool Ativo
        {
            get; set;
        }
    }

    public partial class Campos
    {
        public int idCampos
        {
            get; set;
        }

        public int idTabelas
        {
            get; set;
        }

        public string Nome
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }

        public bool State
        {
            get; set;
        }

        public int Length
        {
            get; set;
        }

        public int Scale
        {
            get; set;
        }

        public bool IsPK
        {
            get; set;
        }

        public bool Ativo
        {
            get; set;
        }
    }

    public class Acao
    {
        public static String sInclusao => _Titles[1];
        public const int Inclusao = 1;
        public static String sAlteracao => _Titles[2];
        public const int Alteracao = 2;
        public static String sDelecao => _Titles[3];
        public const int Delecao = 3;
        public static String sVisualizacao => _Titles[4];
        public const int Visualizacao = 4;

        private static Dictionary<int, String> _Titles = new Dictionary<int, String>()
        {
            [1] = "Inclusão",
            [2] = "Alteração",
            [3] = "Deleção",
            [4] = "Visualização"
        };

        public static String GetTitle(int pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }
    }

    public class Table
    {
        public Table(Tabelas tbl, IEnumerable<Campos> campos)
        {
            Fields = new List<Field>();
            Name = tbl.Nome;
            Schema = tbl.Esquema;
            PKFieldName = tbl.CampoPK;
            ID = tbl.idTabelas;
            foreach (var fld in campos)
            {
                if (fld.Type == "text")
                    continue;
                Fields.Add(new Field(fld));
            }
            PKField = Fields.FirstOrDefault(f => f.IsPK);
        }

        public string Name
        {
            get;
        }

        public string Schema
        {
            get;
        }

        public string PKFieldName
        {
            get;
        }

        public List<Field> Fields
        {
            get;
        }

        public Field PKField
        {
            get;
        }

        public int ID
        {
            get;
        }
    }

    public class Field
    {
        public Field(Campos fld)
        {
            Name = fld.Nome;
            DBType = fld.Type;
            Type = fld.Type;
            Length = fld.Length;
            Scale = fld.Scale;
            IsPK = fld.IsPK;
            ID = fld.idCampos;
        }

        public string Name
        {
            get;
        }

        public string DBType
        {
            get;
        }

        public string Type
        {
            get;
        }

        public int Length
        {
            get;
        }

        public int Scale
        {
            get;
        }

        public bool IsPK
        {
            get;
        }

        public int ID
        {
            get;
        }
    }
}
