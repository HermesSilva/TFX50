using System;
using System.Linq;
using System.Collections.Generic;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;
using TFX.Core.Service.SVC;
using TFX.Core.Model.Data;
using TFX.Core;
using Projecao.Core.PET.RPTs.Receita;
using System.IO;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.RPT.Model;
using TFX.Core.Model.DIC.RPT;
using TFX.iText7;

namespace Projecao.Core.PET.Consultas
{
    [XRegister(typeof(ConsultasReceitasRule), sCID, typeof(ConsultasReceitasSVC))]
    public class ConsultasReceitasRule : ConsultasReceitasSVC.XRule
    {

        public const String sCID = "67841B4C-660D-4BF6-A0C8-9BC97BDFA795";
        public static Guid gCID = new Guid(sCID);

        public ConsultasReceitasRule()
        {
            ID = gCID;
        }
        protected override String Print(XExecContext pContext, ConsultasReceitasSVC.XDataSet pDataSet)
        {
            if (!Directory.Exists(XDefault.WebTempFolder))
                Directory.CreateDirectory(XDefault.WebTempFolder);
            XTFXDocument doc = new XTFXDocument();
            XRPTModel model = XModelCache.Instance.GetRPT(ReceitaNormalRPT.gCID);
            doc = model.Document;
            String pdfname = Path.Combine(XDefault.WebTempFolder, doc.Root.Name + "-" + Guid.NewGuid().ToString() + doc.Extension);
            using (FileStream fsx = new FileStream(pdfname, FileMode.Create))
            {
                XDocumentManager.CreateDocument(pContext, fsx, doc, pContext.Broker.AuxData[0]);
                return Path.Combine(XDefault.WebTempFolderName, Path.GetFileName(pdfname));
            }
        }
        protected override void BeforeFlush(XExecContext pContext, ConsultasReceitasSVC pModel, ConsultasReceitasSVC.XDataSet pDataSet)
        {
            foreach (ConsultasReceitasSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.State == XTupleState.New))
            {
                tpl.Data = XDefault.Now;
                tpl.ERPxProfissionalID = pContext.Logon.UserID;
            }
        }
    }
}
