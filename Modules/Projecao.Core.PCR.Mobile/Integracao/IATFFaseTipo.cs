using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("IATFFaseTipo")]
    public class IATFFaseTipo 
    {
        [PrimaryKey]
        public Int16 PCRxIATFFaseTipoID {get; set;}

        public String Tipo {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
