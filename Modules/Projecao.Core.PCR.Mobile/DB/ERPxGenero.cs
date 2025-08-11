using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("ERPxGenero")]
    public class ERPxGenero 
    {
        public static String sFeminino => _Titles[(Int16)2];
        public const Int16 Feminino = (Int16)2;
        public static String sNI => _Titles[(Int16)0];
        public const Int16 NI = (Int16)0;
        public static String sMasculino => _Titles[(Int16)1];
        public const Int16 Masculino = (Int16)1;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)2] = "Feminino",
            [(Int16)0] = "NI",
            [(Int16)1] = "Masculino"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public String Designacao {get; set;}

        [PrimaryKey]
        public Int16 ERPxGeneroID {get; set;}

        public String Genero {get; set;}

        public Int32 Invisivel {get; set;}
        public Boolean bInvisivel => Invisivel == 1;
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
