using System;

using TFX.Core;

namespace Projecao.Core.PCR.Modelo
{
    public class APPxErrorLog
    {
        public Guid APPxErrorLogID
        {
            get; set;
        }

        public String Erro
        {
            get; set;
        }

        public String Pilha
        {
            get; set;
        }

        public String Tipo
        {
            get;
            set;
        }

        public String Data
        {
            get; set;
        }

        public DateTime dData
        {
            get
            {
                return DateTime.TryParse(Data, out DateTime d) ? d : XDefault.NullDateTime;
            }
        }

        public String GetKey()
        {
            return Erro.GetHashCode() + "@" + Pilha.GetHashCode();
        }
    }
}