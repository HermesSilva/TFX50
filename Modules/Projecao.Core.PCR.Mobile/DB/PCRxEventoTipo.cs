using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxEventoTipo")]
    public class PCRxEventoTipo 
    {
        public static String sCampeio => _Titles[(Int16)12];
        public const Int16 Campeio = (Int16)12;
        public static String sArvore_Caida => _Titles[(Int16)19];
        public const Int16 Arvore_Caida = (Int16)19;
        public static String sDesmama => _Titles[(Int16)2];
        public const Int16 Desmama = (Int16)2;
        public static String sMudanca_de_Fase => _Titles[(Int16)10];
        public const Int16 Mudanca_de_Fase = (Int16)10;
        public static String sIATF => _Titles[(Int16)7];
        public const Int16 IATF = (Int16)7;
        public static String sCerca_Quebrada => _Titles[(Int16)15];
        public const Int16 Cerca_Quebrada = (Int16)15;
        public static String sOutros => _Titles[(Int16)14];
        public const Int16 Outros = (Int16)14;
        public static String sPonte_com_Problema => _Titles[(Int16)17];
        public const Int16 Ponte_com_Problema = (Int16)17;
        public static String sAborto => _Titles[(Int16)11];
        public const Int16 Aborto = (Int16)11;
        public static String sPesagem => _Titles[(Int16)9];
        public const Int16 Pesagem = (Int16)9;
        public static String sNA => _Titles[(Int16)0];
        public const Int16 NA = (Int16)0;
        public static String sNatimorto => _Titles[(Int16)22];
        public const Int16 Natimorto = (Int16)22;
        public static String sAtoleiro => _Titles[(Int16)18];
        public const Int16 Atoleiro = (Int16)18;
        public static String sMorte => _Titles[(Int16)5];
        public const Int16 Morte = (Int16)5;
        public static String sVenda => _Titles[(Int16)6];
        public const Int16 Venda = (Int16)6;
        public static String sNascimento => _Titles[(Int16)1];
        public const Int16 Nascimento = (Int16)1;
        public static String sParto => _Titles[(Int16)20];
        public const Int16 Parto = (Int16)20;
        public static String sDoenca => _Titles[(Int16)4];
        public const Int16 Doenca = (Int16)4;
        public static String sVala_Enxurrada => _Titles[(Int16)16];
        public const Int16 Vala_Enxurrada = (Int16)16;
        public static String sInclusao => _Titles[(Int16)21];
        public const Int16 Inclusao = (Int16)21;
        public static String sVacinacao => _Titles[(Int16)3];
        public const Int16 Vacinacao = (Int16)3;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)12] = "Campeio",
            [(Int16)19] = "Árvore Caída",
            [(Int16)2] = "Desmama",
            [(Int16)10] = "Mudança de Fase",
            [(Int16)7] = "IATF",
            [(Int16)15] = "Cerca Quebrada",
            [(Int16)14] = "Outros",
            [(Int16)17] = "Ponte com Problema",
            [(Int16)11] = "Aborto",
            [(Int16)9] = "Pesagem",
            [(Int16)0] = "NA",
            [(Int16)22] = "Natimorto",
            [(Int16)18] = "Atoleiro",
            [(Int16)5] = "Morte",
            [(Int16)6] = "Venda",
            [(Int16)1] = "Nascimento",
            [(Int16)20] = "Parto",
            [(Int16)4] = "Doença",
            [(Int16)16] = "Vala Enxurrada",
            [(Int16)21] = "Inclusão",
            [(Int16)3] = "Vacinação"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public Int32 Animal {get; set;}
        public Boolean bAnimal => Animal == 1;

        public String Evento {get; set;}

        [PrimaryKey]
        public Int16 PCRxEventoTipoID {get; set;}

        public Int32 Reproducao {get; set;}
        public Boolean bReproducao => Reproducao == 1;
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
