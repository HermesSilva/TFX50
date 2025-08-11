using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("AnimalLote")]
    public class AnimalLote 
    {
        public Int32 Ano {get; set;}

        public String DataCriacao {get; set;}
        public DateTime dDataCriacao => DateTime.TryParse(DataCriacao, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Lote {get; set;}

        [PrimaryKey]
        public Guid PCRxAnimalLoteID {get; set;}
        public String sPCRxAnimalLoteID => PCRxAnimalLoteID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
