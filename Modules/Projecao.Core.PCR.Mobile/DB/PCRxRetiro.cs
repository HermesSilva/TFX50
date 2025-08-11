using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxRetiro")]
    public class PCRxRetiro 
    {
        public String CoordenadasArea {get; set;}

        public Decimal Latitude {get; set;}

        public Decimal Longitude {get; set;}

        public String Nome {get; set;}

        [PrimaryKey]
        public Int32 PCRxRetiroID {get; set;}

        public Guid SYSxEmpresaID {get; set;}
        public String sSYSxEmpresaID => SYSxEmpresaID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
