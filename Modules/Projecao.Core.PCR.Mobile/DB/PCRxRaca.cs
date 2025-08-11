using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxRaca")]
    public class PCRxRaca 
    {
        public static String sAngus => _Titles[(Int16)1];
        public const Int16 Angus = (Int16)1;
        public static String sAberdeen => _Titles[(Int16)3];
        public const Int16 Aberdeen = (Int16)3;
        public static String sNelore => _Titles[(Int16)2];
        public const Int16 Nelore = (Int16)2;
        public static String sNelore_Colorida => _Titles[(Int16)4];
        public const Int16 Nelore_Colorida = (Int16)4;
        public static String sNI => _Titles[(Int16)0];
        public const Int16 NI = (Int16)0;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)1] = "Angus",
            [(Int16)3] = "Aberdeen",
            [(Int16)2] = "Nelore",
            [(Int16)4] = "Nelore (Colorida)",
            [(Int16)0] = "NI"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        [PrimaryKey]
        public Int16 PCRxRacaID {get; set;}

        public String Raca {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
