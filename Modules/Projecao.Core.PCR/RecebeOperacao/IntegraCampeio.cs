using System;
using System.Collections.Generic;

using Projecao.Core.NTR.DB;
using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;
using Projecao.Core.PCR.ReadOnly;
using Projecao.Core.PCR.Rebanho;

using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Service.Data;
using TFX.Core.Service.Apps;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.NTR.DB.NTRx;
using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraCampeio
    {
        public static void AddCampeio(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (_NTRxMobilePontoDestaque dest = XPersistencePool.Get<_NTRxMobilePontoDestaque>(pContext))
            using (RebanhoSVC.XService rebanho = XServicePool.Get<RebanhoSVC.XService>(pContext))
            using (RebanhoSVC.XService animal = XServicePool.Get<RebanhoSVC.XService>(pContext))
            using (AnimalEventoSVC.XService animalEvento = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            using (TouroEventoSVC.XService eventoTouro = XServicePool.Get<TouroEventoSVC.XService>(pContext))
            using (EventoFazendaSVC.XService eventoFazenda = XServicePool.Get<EventoFazendaSVC.XService>(pContext))
            using (NTRx._NTRxMobileAtividade ativ = XPersistencePool.Get<NTRx._NTRxMobileAtividade>(pContext))
            using (_ISExCodigo codigo = XPersistencePool.Get<_ISExCodigo>(pContext))
            {
                dest.Clear();
                rebanho.Clear();
                codigo.Clear();
                eventoTouro.Clear();
                eventoFazenda.Clear();
                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> tpl in pDataSource)
                {
                    APPxAtividade act = tpl.Item1;
                    APPxAtividadeItem actitem = tpl.Item2;
                    pContext.Logon.CurrentCompanyID = act.FazendaID;
                    Guid destid = Guid.Empty;
                    if (actitem.Foto.IsFull())
                    {
                        if (!ativ.OpenAppend(NTRx.NTRxMobileAtividade.NTRxMobileConfigID, tpl.Item3, NTRx.NTRxMobileAtividade.DataInicial, XOperator.GreaterThanOrEqualTo,
                                             actitem.dData.Date, NTRx.NTRxMobileAtividade.DataFinal, XOperator.LessThan, actitem.dData.Date.AddDays(1)))
                        {
                            NTRxMobileAtividade.XTuple atvtpl = ativ.NewTuple();
                            atvtpl.Distancia = 0;
                            atvtpl.DataInicial = actitem.dData;
                            atvtpl.NTRxMobileConfigID = tpl.Item3;
                            atvtpl.DataFinal = actitem.dData;
                            ativ.Flush();
                        }

                        NTRxMobilePontoDestaque.XTuple dtpl = dest.NewTuple();
                        destid = dtpl.NTRxMobilePontoDestaqueID;
                        dtpl.NTRxMobileConfigID = tpl.Item3;
                        dtpl.Texto = actitem.Observacao;
                        dtpl.Foto = actitem.Foto;
                        dtpl.Latitude = actitem.Latitude;
                        dtpl.Longitude = actitem.Longitude;
                        dtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
                        dtpl.Data = actitem.Data.SafeParse();
                    }
                    if (actitem.PCRxEventoTipoID.In(PCRxEventoTipo.XDefault.Cerca_Quebrada, PCRxEventoTipo.XDefault.Arvore_Caida, PCRxEventoTipo.XDefault.Atoleiro,
                                      PCRxEventoTipo.XDefault.Ponte_com_Problema, PCRxEventoTipo.XDefault.Vala_Enxurrada))
                    {
                        EventoFazendaSVC.XTuple evento = eventoFazenda.NewTuple();
                        evento.PCRxEventoTipoID = actitem.PCRxEventoTipoID != 0 ? actitem.PCRxEventoTipoID : act.PCRxEventoTipoID;
                        evento.Observacao = actitem.Observacao;
                        evento.Data = actitem.Data.SafeParse();
                        evento.NTRxMobilePontoDestaqueID = destid;
                        evento.Resolvido = actitem.Resolvido == 1;
                        evento.Latitude = actitem.Latitude;
                        evento.Longitude = actitem.Longitude;
                        evento.CTLxUsuarioID = pUserID;
                    }

                    if (actitem.PCRxAnimalID.IsEmpty())
                        continue;
                    RebanhoSVC.XTuple animaltpl = null;
                    if (animal.Open(actitem.PCRxAnimalID))
                        animaltpl = animal.Current;

                    if (act.PCRxEventoTipoID == PCRxEventoTipo.XDefault.Campeio)
                    {
                        if (actitem.PCRxEventoTipoID == PCRxEventoTipo.XDefault.Campeio)
                            continue;
                        if (actitem.Natimorto == 1)
                            actitem.PCRxEventoTipoID = PCRxEventoTipo.XDefault.Natimorto;
                        if (actitem.PCRxEventoTipoID == PCRxEventoTipo.XDefault.Nascimento)
                        {
                            eventoTouro.MaxRows = 1;
                            TouroEventoSVC.XDataSet touroDataSet = eventoTouro.Get(TouroEventoSVC.xPCRxAnimalEvento.PCRxAnimalEstadoID, PCRxAnimalEstado.XDefault.Prenha,
                                                                                   TouroEventoSVC.xPCRxAnimalEvento.PCRxAnimalID, actitem.PCRxAnimalID,
                                                                                   TouroEventoSVC.xPCRxEvento.Data, XOperator.GreaterThanOrEqualTo, DateTime.Now.AddMonths(-6).Date);
                            Guid paiid = Guid.Empty;
                            Guid veterinarioID = Guid.Empty;
                            Int16 raca = PCRxRaca.XDefault.Nelore;

                            if (touroDataSet.Count == 1)
                            {
                                paiid = touroDataSet.Current.PCRxReprodutorID;
                                veterinarioID = touroDataSet.Current.SYSxPessoaID;

                                if (touroDataSet.Current.PCRxRacaID != 0)
                                    raca = touroDataSet.Current.PCRxRacaID;
                            }
                            IntegraUtils.AddFilhote(rebanho, animalEvento, codigo, destid, paiid, actitem.PCRxAnimalID, veterinarioID, actitem.ERPxGeneroID, raca, actitem.CodigoBrincoNascimento,
                                       actitem.Data.SafeParse(), animaltpl.PCRxAnimalLoteID, pUserID, actitem.Grupo, actitem.Latitude, actitem.Longitude, actitem.Observacao);

                            IntegraUtils.AddEvento(animalEvento, PCRxEventoTipo.XDefault.Parto, actitem.Data.SafeParse(), actitem.PCRxAnimalID, paiid, veterinarioID, Guid.Empty,
                                                   animaltpl.PCRxAnimalLoteID, Guid.Empty, PCRxAnimalEstado.XDefault.Parida, pUserID, actitem.Grupo, 0, actitem.Latitude, actitem.Longitude, destid, actitem.Observacao);
                        }
                        else
                        {
                            IntegraUtils.AddEvento(animalEvento, actitem.PCRxEventoTipoID != 0 ? actitem.PCRxEventoTipoID : act.PCRxEventoTipoID, actitem.Data.SafeParse(),
                                                   actitem.PCRxAnimalID, Guid.Empty, Guid.Empty, Guid.Empty, animaltpl.PCRxAnimalLoteID, Guid.Empty, animaltpl.PCRxAnimalEstadoID,
                                                   pUserID, actitem.Grupo, 0, actitem.Latitude, actitem.Longitude, destid, actitem.Observacao);
                        }
                    }
                }
                dest.Flush();
                rebanho.Flush();
                codigo.Flush();
                eventoTouro.Flush();
                eventoFazenda.Flush();
                animalEvento.Flush();
            }
        }
    }
}