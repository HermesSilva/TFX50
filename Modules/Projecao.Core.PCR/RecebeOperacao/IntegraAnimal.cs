using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.Modelo;
using Projecao.Core.PCR.Rebanho;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.Apps.Usuario;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraAnimal
    {
        public static void AddAnimal(XExecContext pContext, IEnumerable<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> pDataSource, Guid pUserID)
        {
            using (RebanhoSVC.XService rebanho = XServicePool.Get<RebanhoSVC.XService>(pContext))
            using (RebanhoSVC.XService prebanho = XServicePool.Get<RebanhoSVC.XService>(pContext))
            using (UsuarioLogonSVC.XService ulogon = XServicePool.Get<UsuarioLogonSVC.XService>(pContext))
            using (AnimalEventoSVC.XService animalEvento = XServicePool.Get<AnimalEventoSVC.XService>(pContext))
            using (_ISExCodigo codigo = XPersistencePool.Get<_ISExCodigo>(pContext))
            using (_PCRxAnimalPasto apasto = XPersistencePool.Get<_PCRxAnimalPasto>(pContext))
            using (_PCRxAnimalLote animallote = XPersistencePool.Get<_PCRxAnimalLote>(pContext))
            using (_PCRxDocumento repdoc = XPersistencePool.Get<_PCRxDocumento>(pContext))
            using (_PCRxReprodutor reprod = XPersistencePool.Get<_PCRxReprodutor>(pContext))
            {
                animallote.Clear();
                rebanho.Clear();
                codigo.Clear();
                apasto.Clear();
                animalEvento.Clear();
                reprod.Clear();
                repdoc.Clear();

                ulogon.Open(pUserID);
                pContext.Logon.CurrentCompanyID = ulogon.Current.SYSxEmpresaMatrizID;

                foreach (Tuple<APPxAtividade, APPxAtividadeItem, Guid> item in pDataSource.Where(t => t.Item2.CodigoBrinco.IsFull() && t.Item2.PCRxAnimalID.IsEmpty()))
                {
                    if (prebanho.Open(RebanhoSVC.xISExCodigo.Numero, item.Item2.CodigoBrinco) || item.Item2.Status == 0)
                        continue;
                    item.Item2.PCRxAnimalID = Guid.NewGuid();
                    Int16 gen = item.Item2.ERPxGeneroID;
                    if (item.Item2.ERPxGeneroID == ERPxGenero.XDefault.Touro)
                        gen = ERPxGenero.XDefault.Masculino;
                    Guid loteid = IntegraUtils.AddAnimal(rebanho, codigo, apasto, animallote, item.Item2.PCRxAnimalID, gen, item.Item2.IdadeMeses, item.Item2.PCRxRacaID,
                          item.Item1.Lote, item.Item2.CodigoBrinco, item.Item2.PCRxPastoID, item.Item2.Parida);
                    if (item.Item2.ERPxGeneroID == ERPxGenero.XDefault.Touro)
                    {
                        reprod.NewTuple(item.Item2.PCRxAnimalID);
                        reprod.Current.ApenasSemem = false;
                        repdoc.NewTuple(item.Item2.PCRxAnimalID);
                    }
                    if (item.Item2.Parida)
                    {
                        IntegraUtils.AddFilhote(rebanho, animalEvento, codigo, Guid.Empty, Guid.Empty, item.Item2.PCRxAnimalID, Guid.Empty, 0, item.Item2.PCRxRacaID, "X" + item.Item2.CodigoBrinco,
                                   item.Item2.Data.SafeParse().AddMonths(-4), loteid, pUserID, "", item.Item2.Latitude, item.Item2.Longitude, item.Item2.Observacao);
                        item.Item2.Parida = false;
                    }
                }
                animallote.Flush();
                rebanho.Flush();
                codigo.Flush();
                apasto.Flush();
                animalEvento.Flush();
                reprod.Flush();
                repdoc.Flush();
            }
        }
    }
}