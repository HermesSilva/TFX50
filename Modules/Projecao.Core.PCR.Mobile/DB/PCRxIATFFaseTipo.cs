using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxIATFFaseTipo")]
    public class PCRxIATFFaseTipo 
    {
        public static String sNA => _Titles[(Int16)0];
        public const Int16 NA = (Int16)0;
        public static String sDG3 => _Titles[(Int16)16];
        public const Int16 DG3 = (Int16)16;
        public static String sIntervalo => _Titles[(Int16)1];
        public const Int16 Intervalo = (Int16)1;
        public static String sDG_Final => _Titles[(Int16)17];
        public const Int16 DG_Final = (Int16)17;
        public static String sAplicacao_de_Hormonio => _Titles[(Int16)4];
        public const Int16 Aplicacao_de_Hormonio = (Int16)4;
        public static String sDG1 => _Titles[(Int16)12];
        public const Int16 DG1 = (Int16)12;
        public static String sIATF1 => _Titles[(Int16)11];
        public const Int16 IATF1 = (Int16)11;
        public static String sImplante_Dispositivo => _Titles[(Int16)2];
        public const Int16 Implante_Dispositivo = (Int16)2;
        public static String sIATF3 => _Titles[(Int16)15];
        public const Int16 IATF3 = (Int16)15;
        public static String sIATF2 => _Titles[(Int16)13];
        public const Int16 IATF2 = (Int16)13;
        public static String sRetirada_do_Implante => _Titles[(Int16)3];
        public const Int16 Retirada_do_Implante = (Int16)3;
        public static String sDG2 => _Titles[(Int16)14];
        public const Int16 DG2 = (Int16)14;
        private static Dictionary<Int16, String> _Titles = new Dictionary<Int16, String>()
        {
            [(Int16)0] = "NA",
            [(Int16)16] = "DG-3",
            [(Int16)1] = "Intervalo",
            [(Int16)17] = "DG Final",
            [(Int16)4] = "Aplicação de Hormônio",
            [(Int16)12] = "DG-1",
            [(Int16)11] = "IATF-1",
            [(Int16)2] = "Implante Dispositivo",
            [(Int16)15] = "IATF-3",
            [(Int16)13] = "IATF-2",
            [(Int16)3] = "Retirada do Implante",
            [(Int16)14] = "DG-2"
        };

        public static String GetTitle(Int16 pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public Int32 Operacional {get; set;}
        public Boolean bOperacional => Operacional == 1;

        [PrimaryKey]
        public Int16 PCRxIATFFaseTipoID {get; set;}

        public String Tipo {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
