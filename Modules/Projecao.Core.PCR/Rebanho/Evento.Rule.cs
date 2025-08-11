using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.Rebanho
{
    [XRegister(typeof(EventoRule), sCID, typeof(EventoSVC))]
    public class EventoRule : EventoSVC.XRule
    {
        public const String sCID = "7B345DC0-7458-48B7-83F3-D471753F3604";
        public static Guid gCID = new Guid(sCID);

        public EventoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, EventoSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            foreach (EventoSVC.XTuple tpl in pDataSet)
            {
                switch (tpl.PCRxEventoTipoID)
                {
                    case PCRxEventoTipo.XDefault.Pesagem:
                        tpl.Unidade = "Kg";
                        break;

                    case PCRxEventoTipo.XDefault.IATF:
                        tpl.Unidade = "Score";
                        break;

                    case PCRxEventoTipo.XDefault.Vacinacao:
                        tpl.Unidade = "UN";
                        tpl.Valor = 1;
                        break;
                    //case PCRxEventoTipo.XDefault.Arvore_Caida:
                    //case PCRxEventoTipo.XDefault.Desmama:
                    //case PCRxEventoTipo.XDefault.Mudanca_de_Fase:
                    //case PCRxEventoTipo.XDefault.Cerca_Quebrada:
                    //case PCRxEventoTipo.XDefault.Outros:
                    //case PCRxEventoTipo.XDefault.Ponte_com_Problema:
                    //case PCRxEventoTipo.XDefault.Aborto:
                    //case PCRxEventoTipo.XDefault.Atoleiro:
                    //case PCRxEventoTipo.XDefault.Morte:
                    //case PCRxEventoTipo.XDefault.Venda:
                    //case PCRxEventoTipo.XDefault.Nascimento:
                    //case PCRxEventoTipo.XDefault.Parto:
                    //case PCRxEventoTipo.XDefault.Doenca:
                    //case PCRxEventoTipo.XDefault.Vala_Enxurrada:
                    //case PCRxEventoTipo.XDefault.Inclusao:
                    //case PCRxEventoTipo.XDefault.Vacinacao:

                    default:
                        break;
                }
            }
        }
    }
}