using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxEvento")]
    public class PCRxEvento 
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

        public Guid CTLxUsuarioID {get; set;}
        public String sCTLxUsuarioID => CTLxUsuarioID.ToString().ToUpper();

        public String Data {get; set;}
        public DateTime dData => DateTime.TryParse(Data, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public Decimal Latitude {get; set;}

        public Decimal Longitude {get; set;}

        public Guid NTRxMobilePontoDestaqueID {get; set;}
        public String sNTRxMobilePontoDestaqueID => NTRxMobilePontoDestaqueID.ToString().ToUpper();

        public String Observacao {get; set;}

        [PrimaryKey]
        public Guid PCRxEventoID {get; set;}
        public String sPCRxEventoID => PCRxEventoID.ToString().ToUpper();

        public Int16 PCRxEventoTipoID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
