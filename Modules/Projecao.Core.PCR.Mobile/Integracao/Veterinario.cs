using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Veterinario")]
    public class Veterinario 
    {
        public String Nome {get; set;}

        [PrimaryKey]
        public Guid SYSxPessoaID {get; set;}
        public String sSYSxPessoaID => SYSxPessoaID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
