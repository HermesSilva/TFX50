using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("FazendaEndereco")]
    public class FazendaEndereco 
    {
        [PrimaryKey]
        public Int32 ERPxEnderecoID {get; set;}

        public Decimal Latitude {get; set;}

        public Decimal Longitude {get; set;}

        public Guid SYSxPessoaID {get; set;}
        public String sSYSxPessoaID => SYSxPessoaID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
