using System;

namespace Projecao.Core.PCR.Modelo
{
    public class APPxConfig
    {
        public Guid APPxConfigID
        {
            get; set;
        }

        public Boolean EnviarDeletar
        {
            get; set;
        }

        public Boolean ApenasWiFi
        {
            get; set;
        }

        public Int32 IntervaloRefresh
        {
            get; set;
        }

        public Int32 TCPPorta
        {
            get; set;
        }

        public Int32 CRC
        {
            get; set;
        }

        public String Host
        {
            get; set;
        }

        public String Dispositivo
        {
            get; set;
        }

        public String Usuario
        {
            get; set;
        }

        public String Senha
        {
            get; set;
        }

        public Guid UserID
        {
            get; set;
        }

        public Byte[] Foto
        {
            get; set;
        }
    }
}