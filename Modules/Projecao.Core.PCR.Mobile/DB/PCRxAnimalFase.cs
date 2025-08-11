using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxAnimalFase")]
    public class PCRxAnimalFase 
    {
        public static String sBezerro => _Titles[(Int16)1];
        public const Int16 Bezerro = (Int16)1;
        public static String sGarrote => _Titles[(Int16)4];
        public const Int16 Garrote = (Int16)4;
        public static String sNI => _Titles[(Int16)0];
        public const Int16 NI = (Int16)0;
        public static String sVaca => _Titles[(Int16)5];
        public const Int16 Vaca = (Int16)5;
        public static String sTouro => _Titles[(Int16)6];
        public const Int16 Touro = (Int16)6;
        public static String sBezerra => _Titles[(Int16)2];
        public const Int16 Bezerra = (Int16)2;
        public static String sNovilha => _Titles[(Int16)3];
        public const Int16 Novilha = (Int16)3;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)1] = "Bezerro",
            [(Int16)4] = "Garrote",
            [(Int16)0] = "NI",
            [(Int16)5] = "Vaca",
            [(Int16)6] = "Touro",
            [(Int16)2] = "Bezerra",
            [(Int16)3] = "Novilha"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public String Fase {get; set;}

        [PrimaryKey]
        public Int16 PCRxAnimalFaseID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
