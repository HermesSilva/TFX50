using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxFazenda")]
    public class PCRxFazenda 
    {
        public static String s_00000000000000000000000000000000 => _Titles[new Guid("00000000-0000-0000-0000-000000000000")];
        public static Guid _00000000000000000000000000000000 = new Guid("00000000-0000-0000-0000-000000000000");
        private static Dictionary<Guid, String> _Titles = new Dictionary<Guid, String>()
        {
            [new Guid("00000000-0000-0000-0000-000000000000")] = "00000000-0000-0000-0000-000000000000"
        };

        public static String GetTitle(Guid pIndex)
        {
            if (!_Titles.ContainsKey(pIndex))
                return "";
            return _Titles[pIndex];
        }

        public Int32 CEPxLocalidadeID {get; set;}

        public String CoordenadasArea {get; set;}

        [PrimaryKey]
        public Guid PCRxFazendaID {get; set;}
        public String sPCRxFazendaID => PCRxFazendaID.ToString().ToUpper();

        public Guid SYSxEmpresaID {get; set;}
        public String sSYSxEmpresaID => SYSxEmpresaID.ToString().ToUpper();

        public Int16 SYSxEstadoID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
