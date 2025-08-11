using System;
using System.Linq;
using System.Collections.Generic;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;
using TFX.Core.Service.SVC;
using TFX.Core.Data;
using static Projecao.Core.PET.DB.PETx;
using TFX.Core.Model.DIC.ORM;

namespace Projecao.Core.PET.Consultas
{
    [XRegister(typeof(ConsultasDescritivoRule), sCID, typeof(ConsultasDescritivoSVC))]
    public class ConsultasDescritivoRule : ConsultasDescritivoSVC.XRule
    {

        public const String sCID = "7E3D14A5-B1AD-4468-B71A-F669E54A3B82";
        public static Guid gCID = new Guid(sCID);

        public ConsultasDescritivoRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, ConsultasDescritivoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ConsultasDescritivoSVC.xPETxAtendimento.PETxAtendimentoClasseID, XOperator.In, new[] { PETxAtendimentoClasse.XDefault.Saude, PETxAtendimentoClasse.XDefault.Todas });
        }
    }
}
