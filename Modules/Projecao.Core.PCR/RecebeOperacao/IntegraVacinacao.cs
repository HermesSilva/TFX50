using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;
using Projecao.Core.PCR.Rebanho;

using System;
using System.Collections.Generic;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.Apps.Usuario;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraVacinacao
    {
        public static void AddVacinacao(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (RebanhoSVC.XService rebanho = XServicePool.Get<RebanhoSVC.XService>(pContext))
            using (UsuarioLogonSVC.XService ulogon = XServicePool.Get<UsuarioLogonSVC.XService>(pContext))
            using (_ISExCodigo codigo = XPersistencePool.Get<_ISExCodigo>(pContext))
            using (_PCRxAnimalPasto apasto = XPersistencePool.Get<_PCRxAnimalPasto>(pContext))
            using (_PCRxAnimalLote animallote = XPersistencePool.Get<_PCRxAnimalLote>(pContext))
            using (AnimalEventoSVC.XService eventoAnimal = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            {
                animallote.Clear();
                rebanho.Clear();
                codigo.Clear();
                apasto.Clear();
                eventoAnimal.Clear();

                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> tpl in pDataSource)
                {
                    APPxAtividade act = tpl.Item1;
                    APPxAtividadeItem actitem = tpl.Item2;

                    if (actitem.Vacinas.IsFull() && actitem.PCRxAnimalID != Guid.Empty)
                    {
                        String[] vacs = actitem.Vacinas.SafeBreak("|");
                        foreach (String vac in vacs)
                        {
                            if (Guid.TryParse(vac, out Guid vacid))
                                IntegraUtils.AddEvento(eventoAnimal, PCRxEventoTipo.XDefault.Vacinacao, actitem.Data.SafeParse(), actitem.PCRxAnimalID,
                                                       Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, vacid, PCRxAnimalEstado.XDefault.NA,
                                                       pUserID, actitem.Grupo, 0, actitem.Latitude, actitem.Longitude, Guid.Empty, actitem.Observacao);
                        }
                        if (tpl.Item2.Parida)
                        {
                            if (rebanho.OpenAppend(actitem.PCRxAnimalID))
                            {
                                rebanho.Current.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Parida;
                                rebanho.Current.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Vaca;
                            }
                            IntegraUtils.AddFilhote(rebanho, eventoAnimal, codigo, Guid.Empty, Guid.Empty, tpl.Item2.PCRxAnimalID, Guid.Empty, 0, tpl.Item2.PCRxRacaID, "X" + tpl.Item2.CodigoBrinco,
                                                    tpl.Item2.Data.SafeParse().AddMonths(-4), Guid.Empty, pUserID, "", tpl.Item2.Latitude, tpl.Item2.Longitude, tpl.Item2.Observacao);
                        }
                    }
                }
                animallote.Flush();
                rebanho.Flush();
                codigo.Flush();
                apasto.Flush();
                eventoAnimal.Flush();
            }
        }
    }
}