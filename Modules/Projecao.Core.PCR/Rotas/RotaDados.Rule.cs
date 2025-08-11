using Projecao.Core.NTR.DB;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using TFX.Core;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.Rotas
{
    [XRegister(typeof(RotaDadosRule), sCID, typeof(RotaDadosSVC))]
    public class RotaDadosRule : RotaDadosSVC.XRule
    {
        public const String sCID = "6AA1A2E1-7510-418F-8D6A-62FCDAFA7A93";
        public static Guid gCID = new Guid(sCID);

        public RotaDadosRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, RotaDadosSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            NTRx._NTRxMobilePontoDestaque dest = GetTable<NTRx._NTRxMobilePontoDestaque>();
            PontoDestaqueSVC.XService dessvc = GetService<PontoDestaqueSVC.XService>();
            _ERPxEndereco end = GetTable<_ERPxEndereco>();
            _PCRxFazenda faz = GetTable<_PCRxFazenda>();
            Decimal lgi = -1;
            Decimal lti = -1;

            if (end.Open(ERPxEndereco.SYSxPessoaID, pContext.Logon.CurrentCompanyID))
            {
                ERPxEndereco.XTuple etpl = end.FirstOrDefault(t => t.Latitude != 0 && t.Longitude != 0);
                if (etpl != null)
                {
                    lti = etpl.Longitude;
                    lgi = etpl.Latitude;
                }
            }
            String mapa = "";
            if (faz.Open(pContext.Logon.CurrentCompanyID))
                mapa = faz.Current.CoordenadasArea;

            foreach (RotaDadosSVC.XTuple tpl in pDataSet)
            {
                DateTime dtini = tpl.DataInicial.Date;
                String imgfolder = Path.Combine(XDefault.WebTempFolder, tpl.NTRxMobileAtividadeID.AsString());
                tpl.Latitude = lgi;
                tpl.Longitude = lti;
                tpl.CoordenadasArea = mapa;
                if (!Directory.Exists(imgfolder) || tpl.DataFinal.Date.AddDays(2) >= DateTime.Now.Date)
                {
                    Directory.CreateDirectory(imgfolder);
                    if (dest.Open(NTRx.NTRxMobilePontoDestaque.NTRxMobileConfigID, tpl.NTRxMobileConfigID, NTRx.NTRxMobilePontoDestaque.Data, XOperator.GreaterThanOrEqualTo, dtini.Date,
                                                                                                           NTRx.NTRxMobilePontoDestaque.Data, XOperator.LessThan, dtini.Date.AddDays(1)))
                    {
                        String[] files = Directory.GetFiles(imgfolder);
                        if (files.Length != dest.Count)
                            foreach (NTRx.NTRxMobilePontoDestaque.XTuple pttpl in dest)
                                File.WriteAllBytes(Path.Combine(imgfolder, pttpl.NTRxMobilePontoDestaqueID.AsString() + ".jpg"), pttpl.Foto);
                    }
                }
                List<String> data = new List<String>();
                if (dessvc.Open(PontoDestaqueSVC.xNTRxMobilePontoDestaque.NTRxMobileConfigID, tpl.NTRxMobileConfigID, PontoDestaqueSVC.xNTRxMobilePontoDestaque.Data, XOperator.GreaterThanOrEqualTo, dtini.Date,
                                                                                                           PontoDestaqueSVC.xNTRxMobilePontoDestaque.Data, XOperator.LessThan, dtini.Date.AddDays(1)))
                {
                    foreach (IGrouping<Guid, PontoDestaqueSVC.XTuple> grptpl in dessvc.DataSet.Tuples.GroupBy(t => t.NTRxMobilePontoDestaqueID))
                    {
                        String evt = String.Join("/", grptpl.Select(t => t.Evento));
                        PontoDestaqueSVC.XTuple pttpl = grptpl.FirstOrDefault();
                        data.Add($"<div style='width:250px'>" +
                        $"<h3 style='margin: 4px 0'>{evt}</h3>" +
                        $"<div>{pttpl.Data.ToString("dd/MM/yyyy HH:mm")}</div>" +
                        $"{(pttpl.Texto.IsFull() ? $"<div style='margin: 5px 0'>{pttpl.Texto}</div>" : "")}" +
                        $"<img src='{XDefault.WebTempFolderName}/{tpl.NTRxMobileAtividadeID.AsString()}/{pttpl.NTRxMobilePontoDestaqueID.AsString()}.jpg' width='250px'>" +
                        $"</div>|{pttpl.Latitude.ToString(CultureInfo.InvariantCulture) }|{pttpl.Longitude.ToString(CultureInfo.InvariantCulture)}");
                    }
                }
                tpl.Pontos = String.Join(XEnvironment.NewLine, data);
            }
        }
    }
}