using System;
using System.Collections.Generic;

using Projecao.Core.NTR.DB;
using Projecao.Core.PCR.Modelo;

using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.NTR.DB.NTRx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraErros
    {
        public static void AddErros(XExecContext pContext, List<Tuple<APPxErrorLog, Guid>> pTuples)
        {
            using (NTRx._NTRxMobileConfig mblcfg = XPersistencePool.Get<NTRx._NTRxMobileConfig>(pContext))
            using (NTRx._NTRxMobileLogErro mblerr = XPersistencePool.Get<NTRx._NTRxMobileLogErro>(pContext))
            {
                foreach (Tuple<APPxErrorLog, Guid> tpl in pTuples)
                {
                    NTRx.NTRxMobileLogErro.XTuple ertpl = mblerr.NewTuple();
                    ertpl.NTRxMobileConfigID = tpl.Item2;
                    ertpl.Data = tpl.Item1.dData;
                    ertpl.Mensagem = tpl.Item1.Erro;
                    ertpl.Pilha = tpl.Item1.Pilha ?? "NI";
                    String tipo = tpl.Item1.Tipo ?? "NI";
                    switch (tipo)
                    {
                        case "Error":
                        case "Fatal":
                            ertpl.NTRxLogTipoID = NTRxLogTipo.XDefault.Erro;
                            break;

                        case "Debug":
                            ertpl.NTRxLogTipoID = NTRxLogTipo.XDefault.Debug;
                            break;

                        case "Warn":
                            ertpl.NTRxLogTipoID = NTRxLogTipo.XDefault.Aviso;
                            break;

                        case "Info":
                        case "Hint":
                        default:
                            ertpl.NTRxLogTipoID = NTRxLogTipo.XDefault.Mensagem;
                            break;
                    }
                }
                mblerr.Flush();
            }
        }
    }
}