using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("NTRxMobileConfig")]
    public class NTRxMobileConfig 
    {

        public Int32 Chamadas {get; set;}

        public String Data {get; set;}
        public DateTime dData => DateTime.TryParse(Data, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Dispositivo {get; set;}

        [PrimaryKey]
        public Guid NTRxMobileConfigID {get; set;}
        public String sNTRxMobileConfigID => NTRxMobileConfigID.ToString().ToUpper();

        public Int16 SYSxEstadoID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
