using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;

using System;
using System.Collections.Generic;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraPesagem
    {
        public static void AddPesagem(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (AnimalEventoSVC.XService eventoAnimal = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            {
                eventoAnimal.Clear();

                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> tpl in pDataSource)
                {
                    APPxAtividade act = tpl.Item1;
                    APPxAtividadeItem actitem = tpl.Item2;

                    if (actitem.PCRxAnimalID != Guid.Empty && actitem.Peso > 0)
                        IntegraUtils.AddEvento(eventoAnimal, PCRxEventoTipo.XDefault.Pesagem, actitem.Data.SafeParse(), actitem.PCRxAnimalID,
                                               Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, PCRxAnimalEstado.XDefault.NA,
                                               pUserID, actitem.Grupo, actitem.Peso, actitem.Latitude, actitem.Longitude, Guid.Empty, actitem.Observacao);
                }

                eventoAnimal.Flush();
            }
        }
    }
}