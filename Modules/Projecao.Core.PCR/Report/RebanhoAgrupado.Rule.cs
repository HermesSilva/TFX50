using System;
using System.Linq;

using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Report
{
    [XRegister(typeof(RebanhoAgrupadoRule), sCID, typeof(RebanhoAgrupadoSVC))]
    public class RebanhoAgrupadoRule : RebanhoAgrupadoSVC.XRule
    {
        public const String sCID = "1F0ACB2A-37D6-4761-B32A-6CE77C6B62C8";
        public static Guid gCID = new Guid(sCID);

        public RebanhoAgrupadoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, RebanhoAgrupadoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            Random r = new Random(50);
            RebanhoSVC.XService animal = GetService<RebanhoSVC.XService>();
            animal.MaxRows = 0;
            animal.FilterZero = true;
            animal.Open(RebanhoSVC.xPCRxReprodutor.ApenasSemem, XOperator.IsNull, XLogic.OR, RebanhoSVC.xPCRxReprodutor.ApenasSemem, false);
            foreach (RebanhoSVC.XTuple tpl in animal.DataSet)
            {
                String grp;
                Int32 meses = (DateTime.Now.Year - tpl.Nascimento.Year) * 12 + (DateTime.Now.Month - tpl.Nascimento.Month);
                RebanhoAgrupadoSVC.XTuple ntpl;
                Int32 gmes;
                switch (meses)
                {
                    case <= 4:
                        grp = "DE 0 À 4 MESES";
                        gmes = 4;
                        break;

                    case > 4 and <= 12:
                        grp = "DE 4 À 12 MESES";
                        gmes = 12;
                        break;

                    case > 12 and <= 24:
                        grp = "DE 12 À 24 MESES";
                        gmes = 24;
                        break;

                    case > 24 and <= 36:
                        grp = "DE 24 À 36 MESES";
                        gmes = 36;
                        break;

                    default:
                        grp = "ACIMA DE 36 MESES";
                        gmes = 48;
                        break;
                }
                ntpl = pDataSet.Tuples.FirstOrDefault(t => t.FaixaEtaria == grp && t.GeneroID == tpl.ERPxGeneroID);
                if (ntpl == null)
                    ntpl = pDataSet.NewTuple();
                ntpl.GeneroID = tpl.ERPxGeneroID;
                ntpl.FaixaEtaria = grp;
                ntpl.Meses = gmes;
                ntpl.Designacao = tpl.Designacao;
                ntpl.Especie = "Bovina";
                ntpl.Quantidade++;
            }
            RebanhoAgrupadoSVC.XTuple[] tpls = pDataSet.Tuples.OrderBy(t => t.Meses).ThenBy(t => t.Designacao).ToArray();
            pDataSet.ReplaceTuples(tpls);
        }
    }
}