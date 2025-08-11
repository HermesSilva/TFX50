using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;

using System;
using System.Collections.Generic;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraDesmama
    {
        public static void AddDesmama(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (AnimalEventoSVC.XService eventoAnimal = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            {
                eventoAnimal.Clear();
                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> tpl in pDataSource)
                {
                    APPxAtividade act = tpl.Item1;
                    APPxAtividadeItem actitem = tpl.Item2;

                    if ((act.PCRxEventoTipoID == PCRxEventoTipo.XDefault.Desmama || actitem.Desmama) && actitem.PCRxAnimalID != Guid.Empty)
                        IntegraUtils.AddEvento(eventoAnimal, PCRxEventoTipo.XDefault.Desmama, actitem.Data.SafeParse(), actitem.PCRxAnimalID,
                                               Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, PCRxAnimalEstado.XDefault.NA,
                                               pUserID, actitem.Grupo, 0, actitem.Latitude, actitem.Longitude, Guid.Empty, actitem.Observacao);
                }
                eventoAnimal.Flush();
            }
        }
    }
}