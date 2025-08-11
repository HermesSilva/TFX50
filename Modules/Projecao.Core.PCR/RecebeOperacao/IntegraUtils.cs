using Projecao.Core.PCR.AnimalEvento;
using Projecao.Core.PCR.DB;
using Projecao.Core.PCR.Modelo;
using Projecao.Core.PCR.Rebanho;

using System;
using System.Data.Common;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Utils;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.PCR.DB.PCRx;
using static TFX.Core.Service.Apps.SYSx;

using ISExCodigo = Projecao.Core.ISE.DB.ISEx.ISExCodigo;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public class IntegraUtils
    {
        public static Guid AddAnimal(RebanhoSVC.XService pRebanho, _ISExCodigo pCodigo, _PCRxAnimalPasto pPasto, _PCRxAnimalLote pAnimalLote,
                                      Guid pPCRxAnimalID, Int16 pERPxGeneroID, Int32 pIdadeMeses, Int16 pPCRxRacaID, String pLote, String pCodigoBrinco,
                                      Int32 pPCRxPastoID, Boolean pParida)
        {
            RebanhoSVC.XTuple atpl = pRebanho.NewTuple(pPCRxAnimalID);
            atpl.ERPxGeneroID = pERPxGeneroID;
            if (pParida)
            {
                atpl.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Vaca;
                atpl.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Parida;
            }
            else
            {
                (Int16 Estado, Int16 Fase) data = CalcEstadoFase(atpl.ERPxGeneroID, pIdadeMeses);
                atpl.PCRxAnimalEstadoID = data.Estado;
                atpl.PCRxAnimalFaseID = data.Fase;
            }
            atpl.Nascimento = DateTime.Now.AddMonths(-pIdadeMeses);
            atpl.PCRxRacaID = pPCRxRacaID;
            atpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            Guid loteid = Guid.Empty;
            if (pLote.IsFull())
                loteid = IntegraUtils.AddLote(pAnimalLote, pLote, DateTime.Now);
            atpl.PCRxAnimalLoteID = loteid;
            IntegraUtils.AddCodigo(pCodigo, pPCRxAnimalID, pCodigoBrinco);
            if (pPCRxPastoID > 0)
            {
                PCRxAnimalPasto.XTuple aptpl;
                if (pPasto.OpenAppend(pPCRxAnimalID))
                    aptpl = pPasto.Current;
                else
                    aptpl = pPasto.NewTuple(pPCRxAnimalID);
                aptpl.PCRxPastoID = pPCRxPastoID;
            }
            return loteid;
        }

        public static (Int16 pEstado, Int16 pFase) CalcEstadoFase(Int16 pERPxGeneroID, Int32 pIdadeMeses)
        {
            switch (pERPxGeneroID)
            {
                case ERPxGenero.XDefault.Masculino:
                    switch (pIdadeMeses)
                    {
                        case Int32 n when (n <= 10):
                            return (PCRxAnimalEstado.XDefault.Periodo_de_Mama, PCRxAnimalFase.XDefault.Bezerro);

                        case Int32 n when (n > 10 && n <= 16):
                            return (PCRxAnimalEstado.XDefault.Engorda, PCRxAnimalFase.XDefault.Garrote);

                        default:
                            return (PCRxAnimalEstado.XDefault.Engorda, PCRxAnimalFase.XDefault.Touro);
                    }

                case ERPxGenero.XDefault.Feminino:
                    switch (pIdadeMeses)
                    {
                        case Int32 n when (n <= 10):
                            return (PCRxAnimalEstado.XDefault.Periodo_de_Mama, PCRxAnimalFase.XDefault.Bezerra);

                        case Int32 n when (n > 10 && n <= 16):
                            return (PCRxAnimalEstado.XDefault.Engorda, PCRxAnimalFase.XDefault.Novilha);

                        default:
                            return (PCRxAnimalEstado.XDefault.Solteira, PCRxAnimalFase.XDefault.Vaca);
                    }
                default:
                    return (PCRxAnimalEstado.XDefault.NA, PCRxAnimalFase.XDefault.NI);
            }
        }

        public static void AddFilhote(RebanhoSVC.XService pRebanho, AnimalEventoSVC.XService pEvento, _ISExCodigo pCodigo, Guid pDestaque, Guid pPaiID,
                                       Guid pMaeID, Guid pVeterinarioID, Int16 pERPxGeneroID, Int16 pRaca, String pBrinco, DateTime pNascimento, Guid pPCRxAnimalLoteID,
                                       Guid pUserID, String pGrupo, Decimal pLatitude, Decimal pLongitude, String pObservacao)
        {
            RebanhoSVC.XTuple filhote = pRebanho.NewTuple();
            filhote.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            filhote.PCRxPaiID = pPaiID;
            filhote.PCRxMaeID = pMaeID;
            filhote.PCRxRacaID = pRaca;
            filhote.Raca = PCRxRaca.XDefault.GetTitle(pRaca);
            filhote.ERPxGeneroID = pERPxGeneroID;
            filhote.PCRxAnimalFaseID = pERPxGeneroID == ERPxGenero.XDefault.Masculino ? PCRxAnimalFase.XDefault.Bezerro : PCRxAnimalFase.XDefault.Bezerra;
            filhote.Fase = PCRxAnimalFase.XDefault.GetTitle(filhote.PCRxAnimalFaseID);
            filhote.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Periodo_de_Mama;
            IntegraUtils.AddCodigo(pCodigo, filhote.ISExItemID, pBrinco);
            filhote.Nascimento = pNascimento;

            IntegraUtils.AddEvento(pEvento, PCRxEventoTipo.XDefault.Nascimento, pNascimento, filhote.ISExItemID, pPaiID, pVeterinarioID, Guid.Empty,
                                   pPCRxAnimalLoteID, Guid.Empty, PCRxAnimalEstado.XDefault.Periodo_de_Mama, pUserID, pGrupo, 0, pLatitude, pLongitude,
                                   pDestaque, pObservacao);
        }

        public static void Read(DbDataReader pReader, Object pTupla)
        {
            for (int i = 0; i < pReader.FieldCount; i++)
                XUtils.SetValue(pTupla, pReader.GetName(i), pReader.GetValue(i));
        }

        public static void AddEvento(AnimalEventoSVC.XService pEvtAnimal, Int16 pPCRxEventoTipoID, DateTime pData, Guid pPCRxAnimalID, Guid pPCRxReprodutorID, Guid pSYSxPessoaID, Guid pPCRxIATFFaseID,
                                    Guid pPCRxAnimalLoteID, Guid pISExItemID, Int16 pPCRxAnimalEstadoID, Guid pUsusarioID, String pGrupo, Decimal pValor, Decimal pLat = default, Decimal pLon = default,
                                    Guid pNTRxMobilePontoDestaqueID = default, String pObservacao = null)
        {
            AnimalEventoSVC.XTuple evttpl = pEvtAnimal.NewTuple();

            evttpl.PCRxEventoTipoID = pPCRxEventoTipoID;
            evttpl.Data = pData;
            evttpl.Observacao = pObservacao;
            evttpl.NTRxMobilePontoDestaqueID = pNTRxMobilePontoDestaqueID;
            evttpl.Latitude = pLat;
            evttpl.Longitude = pLon;
            evttpl.PCRxAnimalID = pPCRxAnimalID;
            evttpl.PCRxReprodutorID = pPCRxReprodutorID;
            evttpl.SYSxPessoaID = pSYSxPessoaID;
            evttpl.PCRxIATFFaseID = pPCRxIATFFaseID;
            evttpl.PCRxAnimalLoteID = pPCRxAnimalLoteID;
            evttpl.ISExItemID = pISExItemID;
            evttpl.CTLxUsuarioID = pUsusarioID;
            evttpl.PCRxAnimalEstadoID = pPCRxAnimalEstadoID;
            evttpl.Valor = pValor;
        }

        public static Guid AddLote(_PCRxAnimalLote pAnimalLote, String pLote, DateTime pDate)
        {
            PCRxAnimalLote.XTuple tpl;
            if (pAnimalLote.OpenAppend(PCRxAnimalLote.Lote, pLote))
                tpl = pAnimalLote.Current;
            else
            {
                tpl = pAnimalLote.NewTuple();
                tpl.Lote = pLote;
                tpl.DataCriacao = DateTime.Now;
            }
            return tpl.PCRxAnimalLoteID;
        }

        public static String AddCodigo(_ISExCodigo pCodigo, Guid pISExItemID, String pNumeroCurto)
        {
            if (pNumeroCurto.IsEmpty())
                throw new XError("Não é permitido inserir brinco co Código vazio.");
            ISExCodigo.XTuple ctpl = pCodigo.NewTuple();
            ctpl.ISExItemID = pISExItemID;
            ctpl.Numero = ctpl.NumeroCurto = pNumeroCurto;
            ctpl.ISExCodigoTipoID = PCRx.ISExCodigoTipo.XDefault.Brinco_VisualRFID;
            return String.Join(", ", pCodigo.Tuples.Where(t => t.ISExItemID == pISExItemID).Select(t => t.NumeroCurto));
        }

        public static AnimalEventoSVC.XTuple AddEvento(AnimalEventoSVC.XService pAnimalEvento, APPxAtividade pAct, APPxAtividadeItem pActItem, Guid pUserID)
        {
            AnimalEventoSVC.XTuple evento = pAnimalEvento.NewTuple();
            evento.PCRxEventoTipoID = pActItem.PCRxEventoTipoID;
            evento.PCRxAnimalID = pActItem.PCRxAnimalID;
            evento.Observacao = pActItem.Observacao;
            evento.Data = pActItem.Data.SafeParse();
            evento.Latitude = pActItem.Latitude;
            evento.Longitude = pActItem.Longitude;
            evento.CTLxUsuarioID = pUserID;
            evento.SYSxPessoaID = pAct.SYSxPessoaID;

            return evento;
        }
    }
}