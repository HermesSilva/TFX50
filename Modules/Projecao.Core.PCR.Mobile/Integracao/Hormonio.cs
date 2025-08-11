using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Hormonio")]
    public class Hormonio 
    {
        public String Fabricacao {get; set;}
        public DateTime dFabricacao => DateTime.TryParse(Fabricacao, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public Guid ISExItemID {get; set;}
        public String sISExItemID => ISExItemID.ToString().ToUpper();

        public Guid ISExLoteID {get; set;}
        public String sISExLoteID => ISExLoteID.ToString().ToUpper();

        public String Nome {get; set;}

        public String Numero {get; set;}

        [PrimaryKey]
        public Int32 PrimaryKeyID {get; set;}

        public String Validade {get; set;}
        public DateTime dValidade => DateTime.TryParse(Validade, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
