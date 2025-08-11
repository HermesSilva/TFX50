using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("NTRxMobileLogErro")]
    public class NTRxMobileLogErro 
    {
        public String Data {get; set;}
        public DateTime dData => DateTime.TryParse(Data, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Mensagem {get; set;}

        public Int16 NTRxLogTipoID {get; set;}

        public Guid NTRxMobileConfigID {get; set;}
        public String sNTRxMobileConfigID => NTRxMobileConfigID.ToString().ToUpper();

        [PrimaryKey]
        public Int32 NTRxMobileLogErroID {get; set;}

        public String Pilha {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
