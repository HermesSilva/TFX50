using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxAnimalEstado")]
    public class PCRxAnimalEstado 
    {
        public static String sPeriodo_de_Mama => _Titles[(Int16)7];
        public const Int16 Periodo_de_Mama = (Int16)7;
        public static String sVendida => _Titles[(Int16)9];
        public const Int16 Vendida = (Int16)9;
        public static String sImplante_Hormonal => _Titles[(Int16)3];
        public const Int16 Implante_Hormonal = (Int16)3;
        public static String sParida => _Titles[(Int16)6];
        public const Int16 Parida = (Int16)6;
        public static String sPrenha => _Titles[(Int16)5];
        public const Int16 Prenha = (Int16)5;
        public static String sNA => _Titles[(Int16)0];
        public const Int16 NA = (Int16)0;
        public static String sInceminado => _Titles[(Int16)4];
        public const Int16 Inceminado = (Int16)4;
        public static String sMorto => _Titles[(Int16)8];
        public const Int16 Morto = (Int16)8;
        public static String sEngorda => _Titles[(Int16)2];
        public const Int16 Engorda = (Int16)2;
        public static String sCrescimento => _Titles[(Int16)1];
        public const Int16 Crescimento = (Int16)1;
        public static String sSolteira => _Titles[(Int16)10];
        public const Int16 Solteira = (Int16)10;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)7] = "Período de Mama",
            [(Int16)9] = "Vendida",
            [(Int16)3] = "Implante Hormonal",
            [(Int16)6] = "Parida",
            [(Int16)5] = "Prenha",
            [(Int16)0] = "NA",
            [(Int16)4] = "Inceminado",
            [(Int16)8] = "Morto",
            [(Int16)2] = "Engorda",
            [(Int16)1] = "Crescimento",
            [(Int16)10] = "Solteira"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public String AnimalEstado {get; set;}

        [PrimaryKey]
        public Int16 PCRxAnimalEstadoID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
