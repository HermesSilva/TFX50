using System;
using System.Collections.Generic;

using SQLite;

namespace Projecao.Core.PCR
{
    [Table("Rebanho")]
    public class Rebanho 
    {
        public String AnimalEstado {get; set;}

        public Int32 ApenasSemem {get; set;}
        public Boolean bApenasSemem => ApenasSemem == 1;

        public String DataCriacao {get; set;}
        public DateTime dDataCriacao => DateTime.TryParse(DataCriacao, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public Int16 ERPxGeneroID {get; set;}

        public String Fase {get; set;}

        public String Genero {get; set;}

        public Int32 IdadeMeses {get; set;}

        public String Lote {get; set;}

        public String Nascimento {get; set;}
        public DateTime dNascimento => DateTime.TryParse(Nascimento, out DateTime dt) ? dt : new DateTime(1755, 1, 1, 0, 0, 0);

        public String Nome {get; set;}

        public String NomeMae {get; set;}

        public String NomePai {get; set;}

        public Int16 PCRxAnimalEstadoID {get; set;}

        public Int16 PCRxAnimalFaseID {get; set;}

        [PrimaryKey]
        public Guid PCRxAnimalID {get; set;}
        public String sPCRxAnimalID => PCRxAnimalID.ToString().ToUpper();

        public Guid PCRxAnimalLoteID {get; set;}
        public String sPCRxAnimalLoteID => PCRxAnimalLoteID.ToString().ToUpper();

        public Guid PCRxIATFFaseID {get; set;}
        public String sPCRxIATFFaseID => PCRxIATFFaseID.ToString().ToUpper();

        public Int16 PCRxIATFFaseTipoID {get; set;}

        public Guid PCRxMaeID {get; set;}
        public String sPCRxMaeID => PCRxMaeID.ToString().ToUpper();

        public Guid PCRxPaiID {get; set;}
        public String sPCRxPaiID => PCRxPaiID.ToString().ToUpper();

        public Int16 PCRxRacaID {get; set;}

        public String Raca {get; set;}

        public Guid SYSxEmpresaID {get; set;}
        public String sSYSxEmpresaID => SYSxEmpresaID.ToString().ToUpper();
        [Ignore]
        public Boolean Selected {get; set;}
        [Ignore]
        public Int32 Order {get; set;}
    }
}
