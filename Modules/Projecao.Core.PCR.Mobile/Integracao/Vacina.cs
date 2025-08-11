using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Vacina")]
    public class Vacina 
    {
        [PrimaryKey]
        public Guid ISExItemID {get; set;}
        public String sISExItemID => ISExItemID.ToString().ToUpper();

        public String Nome {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
