using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.Reprodutor
{
    [XRegister(typeof(ReprodutorRule), sCID, typeof(ReprodutorSVC))]
    public class ReprodutorRule : ReprodutorSVC.XRule
    {
        public const String sCID = "2FD80DCD-B3A2-46D5-A4C1-98A863BFB839";
        public static Guid gCID = new Guid(sCID);

        public ReprodutorRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, ReprodutorSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(ReprodutorSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Reprodutor_Bovino);
            pWhere.Add(ReprodutorSVC.xISExCodigo.ISExCodigoTipoID, PCRx.ISExCodigoTipo.XDefault.Brinco_VisualRFID);
        }

        protected override void BeforeFlush(XExecContext pContext, ReprodutorSVC pModel, ReprodutorSVC.XDataSet pDataSet)
        {
            foreach (ReprodutorSVC.XTuple tpl in pDataSet)
            {
                tpl.PCRxAnimalFaseID = PCRxAnimalFase.XDefault.Touro;
                tpl.ERPxGeneroID = ERPxGenero.XDefault.Masculino;
                tpl.NumeroCurto = tpl.Numero;
                tpl.ISExCodigoTipoID = ISExCodigoTipo.XDefault.Brinco_VisualRFID;
            }
        }
    }
}