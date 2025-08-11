using System;

namespace Projecao.Core.PCR.Modelo
{
    public class APPxAtividade
    {
        public APPxAtividade()
        {
            APPxAtividadeID = Guid.NewGuid();
        }

        public String Vacinas
        {
            get; set;
        }

        public Boolean Parida
        {
            get; set;
        }

        public Guid APPxAtividadeID
        {
            get; set;
        }

        public Guid FazendaID
        {
            get; set;
        }

        public Guid MobileID
        {
            get; set;
        }

        public Boolean PermiteIncluirAnimal
        {
            get; set;
        }

        public Int32 QuantidadeTotal
        {
            get; set;
        }

        public Int32 PesoTotal
        {
            get; set;
        }

        public String DataAbertura
        {
            get; set;
        }

        public Decimal LatitudeAbertura
        {
            get; set;
        }

        public Decimal LongitudeAbertura
        {
            get; set;
        }

        public Decimal PrecisaoAbertura
        {
            get; set;
        }

        public String DataFinalizacao
        {
            get; set;
        }

        public Decimal LatitudeFinalizacao
        {
            get; set;
        }

        public Decimal LongitudeFinalizacao
        {
            get; set;
        }

        public Decimal PrecisaoFinalizacao
        {
            get; set;
        }

        public Int32 Status
        {
            get; set;
        }

        public Int16 PCRxEventoTipoID
        {
            get; set;
        }

        public Guid PCRxHormonioID
        {
            get; set;
        }

        public Guid PCRxAnimalID
        {
            get; set;
        }

        public Guid SYSxPessoaID
        {
            get; set;
        }

        public Int16 ERPxGeneroID
        {
            get; set;
        }

        public Int16 PCRxRacaID
        {
            get; set;
        }

        public Int16 IdadeMeses
        {
            get; set;
        }

        public Int16 PCRxIATFFaseTipoID
        {
            get; set;
        }

        public Guid ImplanteIATFID
        {
            get; set;
        }

        public Guid PCRxAnimalLoteID
        {
            get; set;
        }

        public String Lote
        {
            get; set;
        }

        public string Grupo
        {
            get; set;
        }
    }
}