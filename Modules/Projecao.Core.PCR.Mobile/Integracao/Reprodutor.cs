using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Reprodutor")]
    public class Reprodutor 
    {
        public Int32 ApenasSemem {get; set;}
        public Boolean bApenasSemem => ApenasSemem == 1;

        public String Nascimento {get; set;}
        public DateTime dNascimento => DateTime.TryParse(Nascimento, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Nome {get; set;}

        [PrimaryKey]
        public Guid PCRxAnimalID {get; set;}
        public String sPCRxAnimalID => PCRxAnimalID.ToString().ToUpper();

        public String Raca {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
