using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("PCRxAnimalFiliacao")]
    public class PCRxAnimalFiliacao 
    {

        [PrimaryKey]
        public Guid PCRxAnimalFiliacaoID {get; set;}
        public String sPCRxAnimalFiliacaoID => PCRxAnimalFiliacaoID.ToString().ToUpper();

        public Guid PCRxMaeID {get; set;}
        public String sPCRxMaeID => PCRxMaeID.ToString().ToUpper();

        public Guid PCRxPaiID {get; set;}
        public String sPCRxPaiID => PCRxPaiID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
