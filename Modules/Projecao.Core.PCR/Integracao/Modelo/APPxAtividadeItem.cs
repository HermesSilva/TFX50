using System;

namespace Projecao.Core.PCR.Modelo
{
    public class APPxAtividadeItem
    {
        public APPxAtividadeItem()
        {
            APPxAtividadeItemID = Guid.NewGuid();
        }

        public String Vacinas
        {
            get; set;
        }

        public Boolean Parida
        {
            get; set;
        }

        public Guid APPxAtividadeItemID
        {
            get; set;
        }

        public Int32 PCRxPastoID
        {
            get; set;
        }

        public Guid APPxAtividadeID
        {
            get; set;
        }

        public String Data
        {
            get; set;
        }

        public DateTime dData
        {
            get
            {
                return DateTime.Parse(Data);
            }
        }

        public Decimal Latitude
        {
            get; set;
        }

        public Decimal Longitude
        {
            get; set;
        }

        public Decimal Precisao
        {
            get; set;
        }

        public Int16 PCRxEventoTipoID
        {
            get; set;
        }

        public Guid PCRxAnimalID
        {
            get; set;
        }

        public String CodigoBrinco
        {
            get; set;
        }

        public String CodigoBrincoNascimento
        {
            get; set;
        }

        public string NomeAnimal
        {
            get; set;
        }

        public String Observacao
        {
            get; set;
        }

        public Int32 Peso
        {
            get; set;
        }

        public Byte[] Foto
        {
            get; set;
        }

        public Int32 Status
        {
            get; set;
        }

        public Int32 Resolvido
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

        public Int16 Natimorto
        {
            get; set;
        }

        public Boolean Gravida
        {
            get; set;
        }

        public Boolean Desmama
        {
            get; set;
        }

        public Decimal Ecc
        {
            get; set;
        }

        public Guid PCRxHormonioID
        {
            get; set;
        }

        public Guid ImplanteIATFID
        {
            get; set;
        }

        public Guid PCRxReprodutorID
        {
            get; set;
        }

        public string Grupo
        {
            get; set;
        }
    }
}