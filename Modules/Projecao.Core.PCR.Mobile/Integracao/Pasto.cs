using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Pasto")]
    public class Pasto 
    {
        public Int32 Area {get; set;}

        public String CoordenadasArea {get; set;}

        public Decimal Latitude {get; set;}

        public Decimal Longitude {get; set;}

        public String Nome {get; set;}

        public String NomeRetiro {get; set;}

        [PrimaryKey]
        public Int32 PCRxPastoID {get; set;}

        public Int32 PCRxRetiroID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
