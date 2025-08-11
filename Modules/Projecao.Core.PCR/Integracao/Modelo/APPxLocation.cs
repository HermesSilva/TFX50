using System;

namespace Projecao.Core.PCR.Modelo
{
    public class APPxLocation
    {
        public APPxLocation()
        {
            Ordem++;
        }

        public Guid APPxLocationID
        {
            get; set;
        }

        public Int32 Ordem
        {
            get; set;
        }

        public Int64 Ticks
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

        public String MobileID
        {
            get; set;
        }

        public Double Latitude
        {
            get; set;
        }

        public Double Longitude
        {
            get; set;
        }

        public Double Altitude
        {
            get; set;
        }

        public Decimal Precisao
        {
            get; set;
        }

        public Decimal Velocidade
        {
            get; set;
        }
    }
}