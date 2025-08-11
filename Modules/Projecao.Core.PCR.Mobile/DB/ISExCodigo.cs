using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("ISExCodigo")]
    public class ISExCodigo 
    {

        [PrimaryKey]
        public Guid ISExCodigoID {get; set;}
        public String sISExCodigoID => ISExCodigoID.ToString().ToUpper();

        public Int16 ISExCodigoTipoID {get; set;}

        public Guid ISExItemID {get; set;}
        public String sISExItemID => ISExItemID.ToString().ToUpper();

        public String Numero {get; set;}

        public String NumeroCurto {get; set;}
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
