using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;

using System;
using System.Collections.Generic;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraIATF
    {
        public static void AddIATF(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (AnimalEventoSVC.XService eventoAnimal = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            using (_PCRxAnimalLote animalLote = XPersistencePool.Get<_PCRxAnimalLote>(pContext))
            {
                eventoAnimal.Clear();
                animalLote.Clear();

                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> tpl in pDataSource)
                {
                    APPxAtividade act = tpl.Item1;
                    APPxAtividadeItem actItem = tpl.Item2;

                    if (act.PCRxEventoTipoID == PCRxEventoTipo.XDefault.IATF && actItem.PCRxAnimalID != Guid.Empty)
                    {
                        AnimalEventoSVC.XTuple evento = AddEvento(eventoAnimal, animalLote, act, actItem, pUserID);

                        if (act.PCRxIATFFaseTipoID == PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio)
                            evento.ISExItemID = actItem.PCRxHormonioID;
                        else
                        if (actItem.PCRxHormonioID != Guid.Empty)
                        {
                            AnimalEventoSVC.XTuple eventoHormonio = AddEvento(eventoAnimal, animalLote, act, actItem, pUserID);
                            eventoHormonio.PCRxIATFFaseTipoID = PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio;
                            eventoHormonio.ISExItemID = actItem.PCRxHormonioID;
                        }

                        if (act.PCRxIATFFaseTipoID == PCRxIATFFaseTipo.XDefault.Implante_Dispositivo)
                            evento.ISExItemID = actItem.ImplanteIATFID;
                        else
                        if (actItem.ImplanteIATFID != Guid.Empty)
                        {
                            AnimalEventoSVC.XTuple eventoImplante = AddEvento(eventoAnimal, animalLote, act, actItem, pUserID);
                            eventoImplante.PCRxIATFFaseTipoID = PCRxIATFFaseTipo.XDefault.Implante_Dispositivo;
                            eventoImplante.ISExItemID = actItem.ImplanteIATFID;
                        }

                        if (act.PCRxIATFFaseTipoID.In(PCRxIATFFaseTipo.XDefault.IATF1, PCRxIATFFaseTipo.XDefault.IATF2, PCRxIATFFaseTipo.XDefault.IATF3))
                            evento.PCRxReprodutorID = actItem.PCRxReprodutorID;
                        else
                        if (act.PCRxIATFFaseTipoID.In(PCRxIATFFaseTipo.XDefault.DG1, PCRxIATFFaseTipo.XDefault.DG2, PCRxIATFFaseTipo.XDefault.DG3, PCRxIATFFaseTipo.XDefault.DG_Final))
                            evento.PCRxAnimalEstadoID = actItem.Gravida ? PCRxAnimalEstado.XDefault.Prenha : PCRxAnimalEstado.XDefault.NA;
                    }
                }

                animalLote.Flush();
                eventoAnimal.Flush();
            }
        }

        private static AnimalEventoSVC.XTuple AddEvento(AnimalEventoSVC.XService pAnimalEvento, _PCRxAnimalLote pAnimalLote, APPxAtividade pAct, APPxAtividadeItem pActItem, Guid pUserID)
        {
            Guid loteid = Guid.Empty;
            if (pAct.Lote.IsFull())
                loteid = IntegraUtils.AddLote(pAnimalLote, pAct.Lote, DateTime.Now);
            AnimalEventoSVC.XTuple evento = IntegraUtils.AddEvento(pAnimalEvento, pAct, pActItem, pUserID);
            if (loteid.IsFull())
                evento.PCRxAnimalLoteID = loteid;
            evento.PCRxIATFFaseTipoID = pAct.PCRxIATFFaseTipoID;
            evento.Valor = pActItem.Ecc / 10.0M;

            return evento;
        }
    }
}