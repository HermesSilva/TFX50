using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.PCR.DB.PCRx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PCR.Rebanho
{
    [XRegister(typeof(RebanhoRule), sCID, typeof(RebanhoSVC))]
    public class RebanhoRule : RebanhoSVC.XRule
    {
        public const String sCID = "F8F59E9B-6C53-4E98-B899-11C0804A5BC8";
        public static Guid gCID = new Guid(sCID);

        public RebanhoRule()
        {
            ID = gCID;
        }


        private Random _Rnd = new Random();
        private HashSet<String> _Cod = new HashSet<String>();

        protected override void GetWhere(XExecContext pContext, RebanhoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(RebanhoSVC.xISExCodigo.ISExCodigoTipoID, PCRx.ISExCodigoTipo.XDefault.Brinco_VisualRFID);
        }

        protected override void BeforeFlush(XExecContext pContext, RebanhoSVC pModel, RebanhoSVC.XDataSet pDataSet)
        {
            foreach (RebanhoSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.Nome.IsEmpty()))
                tpl.Nome = tpl.Fase + " " + tpl.Raca;
        }
        protected override String ExecuteCustom(XExecContext pContext, RebanhoSVC.XDataSet pDataSet)
        {
            RebanhoSVC.XService rebanho = GetService<RebanhoSVC.XService>();
            _ISExCodigo codigo = GetTable<_ISExCodigo>();
            _PCRxReprodutor reproduto = GetTable<_PCRxReprodutor>();
            Int32 cnt = 1;
            for (int i = 0; i < cnt; i++)
            {
                rebanho.Clear();
                RebanhoSVC.XTuple paitpl = rebanho.NewTuple();
                if (i.In(0, 3))
                {
                    paitpl.PCRxRacaID = PCRxRaca.XDefault.Angus;
                    paitpl.Raca = PCRxRaca.XDefault.sAngus;
                }
                else
                {
                    paitpl.PCRxRacaID = PCRxRaca.XDefault.Nelore;
                    paitpl.Raca = PCRxRaca.XDefault.sNelore;
                }
                paitpl.ERPxGeneroID = ERPxGenero.XDefault.Masculino;
                paitpl.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Touro;
                paitpl.Fase = PCRxAnimalFase.XDefault.sTouro;
                paitpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                paitpl.Nascimento = DateTime.Now.AddDays(_Rnd.Next(365 * 2, 365 * 12) * -1);
                PCRxReprodutor.XTuple reptpl = reproduto.NewTuple(paitpl.ISExItemID);
                reptpl.ApenasSemem = false;
                codigo.Clear();
                AddCodigo(codigo, paitpl);
                RebanhoSVC.XTuple maetpl = rebanho.NewTuple();
                maetpl.PCRxRacaID = PCRxRaca.XDefault.Nelore;
                maetpl.Raca = PCRxRaca.XDefault.sNelore;
                maetpl.Nascimento = DateTime.Now.AddDays(_Rnd.Next(365 * 2, 365 * 12) * -1);
                maetpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                maetpl.ERPxGeneroID = ERPxGenero.XDefault.Feminino;
                maetpl.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Vaca;
                maetpl.PCRxAnimalEstadoID = (new[] { PCRxAnimalEstado.XDefault.Parida, PCRxAnimalEstado.XDefault.Inceminado,
                                                      PCRxAnimalEstado.XDefault.Prenha,PCRxAnimalEstado.XDefault.Implante_Hormonal }).Random();
                maetpl.Fase = PCRxAnimalFase.XDefault.sVaca;
                AddCodigo(codigo, maetpl);
                rebanho.Flush();
                reproduto.Flush();
                codigo.Flush();
                CreateNascimento(rebanho, codigo, paitpl, maetpl, i);
            }
            return $"{(cnt * 3).ToString("#,##0")} Animais gerados";
        }

        private String AddCodigo(_ISExCodigo pCodigo, RebanhoSVC.XTuple pTuple)
        {
            ISExCodigo.XTuple ctpl = pCodigo.NewTuple();
            ctpl.ISExItemID = pTuple.ISExItemID;
            ctpl.Numero = ctpl.NumeroCurto = GetNumber();
            ctpl.ISExCodigoTipoID = Projecao.Core.PCR.DB.PCRx.ISExCodigoTipo.XDefault.Brinco_VisualRFID;
            return String.Join(", ", pCodigo.Tuples.Where(t => t.ISExItemID == pTuple.ISExItemID).Select(t => t.NumeroCurto));
        }

        private Int32 _LCod = 1;

        private String GetNumber()
        {
            while (true)
            {
                String cod = (_LCod++).ToString();
                if (!_Cod.Contains(cod))
                    _Cod.Add(cod);
                return cod;
            }
        }

        private void CreateNascimento(RebanhoSVC.XService pRebanho, _ISExCodigo pCodigo, RebanhoSVC.XTuple pPai, RebanhoSVC.XTuple pMae, Int32 pIdx)
        {
            pRebanho.Clear();
            pCodigo.Clear();
            RebanhoSVC.XTuple filhote = pRebanho.NewTuple();
            filhote.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            filhote.PCRxPaiID = pPai.ISExItemID;
            filhote.PCRxMaeID = pMae.ISExItemID;

            if (pPai.PCRxRacaID == PCRxRaca.XDefault.Angus)
            {
                filhote.PCRxRacaID = PCRxRaca.XDefault.Aberdeen;
                filhote.Raca = PCRxRaca.XDefault.sAberdeen;
            }
            else
            {
                filhote.PCRxRacaID = PCRxRaca.XDefault.Nelore;
                filhote.Raca = PCRxRaca.XDefault.sNelore;
            }

            if (pIdx % 2 == 0)
            {
                filhote.ERPxGeneroID = ERPxGenero.XDefault.Masculino;
                filhote.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Bezerro;
                filhote.Fase = PCRxAnimalFase.XDefault.sBezerro;
            }
            else
            {
                filhote.ERPxGeneroID = ERPxGenero.XDefault.Feminino;
                filhote.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Bezerra;
                filhote.Fase = PCRxAnimalFase.XDefault.sBezerra;
            }
            filhote.PCRxAnimalEstadoID = PCRxAnimalEstado.XDefault.Periodo_de_Mama;
            //AnimalEventoSVC.XTuple evento = pRebanho.DataSet.AnimalEventoDataSet.NewTuple();
            //evento.PCRxEventoTipoID = PCRxEventoTipo.XDefault.Nascimento;
            //filhote.Nascimento = evento.Data = DateTime.Now.AddDays(_Rnd.Next(300) * -1);
            AddCodigo(pCodigo, filhote);
            pRebanho.Flush();
            pCodigo.Flush();
        }
    }
}