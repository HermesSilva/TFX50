using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Fazenda")]
    public class Fazenda 
    {
        public Int32 CEPxLocalidadeID {get; set;}

        public String DataBanco {get; set;}
        public DateTime dDataBanco => DateTime.TryParse(DataBanco, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Nome {get; set;}

        [PrimaryKey]
        public Guid PCRxFazendaID {get; set;}
        public String sPCRxFazendaID => PCRxFazendaID.ToString().ToUpper();

        public String Sigla {get; set;}

        public Guid SYSxEmpresaID {get; set;}
        public String sSYSxEmpresaID => SYSxEmpresaID.ToString().ToUpper();

        public Int16 SYSxEstadoID {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
