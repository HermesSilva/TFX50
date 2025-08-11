using System;
using System.IO;

using Projecao.Core.ERP.Pessoa;
using Projecao.Core.ERP.PessoaFisica;
using Projecao.Core.ERP.PessoaJuridica;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.CEP.DB.CEPx;
using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeCliente), sCID)]
    public class RecebeCliente : XClientMessageRule
    {
        private const String sCID = "2CFF2546-4BD0-48D6-AB83-1AB2AAE61B77";
        public static Guid gCID = new Guid(sCID);

        public override void Execute(XExecContext pContext)
        {
            using (_ERPxDocumento doc = XPersistencePool.Get<_ERPxDocumento>(pContext))
            using (_SYSxPessoaEmpresaGrupo peg = XPersistencePool.Get<_SYSxPessoaEmpresaGrupo>(pContext))
            using (_CEPxLocalidade loc = XPersistencePool.Get<_CEPxLocalidade>(pContext))
            using (_CEPxLogradouro log = XPersistencePool.Get<_CEPxLogradouro>(pContext))
            using (_PETxTutor tt = XPersistencePool.Get<_PETxTutor>(pContext))
            using (_ERPxPessoaJuridica pesj = XPersistencePool.Get<_ERPxPessoaJuridica>(pContext))
            using (_ERPxPessoaFisica pesf = XPersistencePool.Get<_ERPxPessoaFisica>(pContext))
            using (PessoaFisicaSVC.XService pesfis = XServicePool.Get<PessoaFisicaSVC.XService>(PessoaFisicaSVC.gCID, pContext))
            using (PessoaJuridicaSVC.XService pesjur = XServicePool.Get<PessoaJuridicaSVC.XService>(PessoaJuridicaSVC.gCID, pContext))
            using (XMemoryStream ms = new XMemoryStream(Data))
            using (StreamReader sr = new StreamReader(ms, XEncoding.Default))
            {
                doc.FilterZero = true;
                peg.FilterZero = true;
                loc.FilterZero = true;
                log.FilterZero = true;
                log.MaxRows = 1;
                Fields = sr.ReadLine().SafeBreak(";");
                while (!sr.EndOfStream)
                {
                    String[] data = sr.ReadLine().SafeBreak(";", StringSplitOptions.None);
                    if (data.IsEmpty())
                        continue;
                    String docnr = GetValue(data, "CAD_CGC").OnlyNumbers();
                    Count++;
                    Int16 doctype = ERPxDocumentoTipo.XDefault.Outros;
                    if (XUtils.CheckCNPJ(docnr))
                        doctype = ERPxDocumentoTipo.XDefault.CNPJ;
                    if (XUtils.CheckCPF(docnr))
                        doctype = ERPxDocumentoTipo.XDefault.CPF;
                    if (doctype == ERPxDocumentoTipo.XDefault.Outros)
                    {
                        Log.Warn($"PESSOA [{GetValue(data, "NOME")}] com documento [{docnr}] INVÁLIDO, NÃO SERÁ INTEGRADO, EMPRESA[{pContext.Logon.CompanyName}].");
                        continue;
                    }
                    Guid pid = Guid.Empty;
                    if (doctype == ERPxDocumentoTipo.XDefault.CPF)
                    {
                        pid = XAppUtils.InsertGrupoCPF(pContext, tt, docnr, true);
                        pid = XAppUtils.InsertGrupoCPF(pContext, pesf, docnr, true);
                    }
                    else
                        pid = XAppUtils.InsertGrupoCNPJ(pContext, pesj, docnr, true);
                    if (pid.IsFull())
                        continue;
                    String ibge = GetValue(data, "CODIGO_MUNICIPIO");
                    if (!loc.Open(CEPxLocalidade.CodigoIBGE, ibge))
                    {
                        Log.Warn($"PESSOA [{GetValue(data, "NOME")}] SEM Código do IBG [{ibge}] VÁLIDO, NÃO SERÁ INTEGRADO, EMPRESA[{pContext.Logon.CompanyName}].");
                        continue;
                    }
                    String cep = GetValue(data, "CEP");
                    if (!log.Open(CEPxLogradouro.CEP, cep))
                        if (!log.Open(CEPxLogradouro.CEP, loc.Current.CEPGeral))
                        {
                            Log.Warn($"PESSOA [{GetValue(data, "NOME")}] SEM CEP [{cep}] VÁLIDO, NÃO SERÁ INTEGRADO, EMPRESA[{pContext.Logon.CompanyName}].");
                            continue;
                        }

                    switch (doctype)
                    {
                        case ERPxDocumentoTipo.XDefault.CNPJ:
                            pesjur.Clear();
                            pesjur.NewTuple();
                            pesjur.Current.RazaoSocial = pesjur.Current.Nome = GetValue(data, "NOME");
                            InsereDetalhes(data, pesjur.DataSet.DocumentoDataSet, pesjur.DataSet.ContatoDataSet, pesjur.DataSet.EnderecoDataSet, log.Current.CEPxLogradouroID, ERPxDocumentoTipo.XDefault.CNPJ);
                            //pesjur.DataSet.ShowData();
                            if (pesjur.DataSet.EnderecoDataSet.Count == 0)
                                pesjur.Clear();
                            else
                                pesjur.Flush();
                            break;

                        case ERPxDocumentoTipo.XDefault.CPF:
                            pesfis.Clear();
                            pesfis.NewTuple();
                            pesfis.Current.Nome = GetValue(data, "NOME");
                            InsereDetalhes(data, pesfis.DataSet.DocumentoDataSet, pesfis.DataSet.ContatoDataSet, pesfis.DataSet.EnderecoDataSet, log.Current.CEPxLogradouroID, ERPxDocumentoTipo.XDefault.CPF);
                            if (pesfis.DataSet.EnderecoDataSet.Count == 0)
                                pesfis.Clear();
                            else
                                pesfis.Flush();
                            break;
                    }
                }
            }
        }

        private void InsereDetalhes(String[] pData, DocumentoSVC.XDataSet pDoc, ContatoSVC.XDataSet pCont, EnderecoSVC.XDataSet pEnd, Int32 pCEPxLogradouroID, Int16 pDocTipo)
        {
            String docnr = GetValue(pData, "CAD_CGC").OnlyNumbers();
            pDoc.NewTuple();
            pDoc.Current.ERPxDocumentoTipoID = pDocTipo;
            pDoc.Current.Numero = docnr;
            String ctnr = GetValue(pData, "TELEFONE");
            if (ctnr.IsFull())
            {
                pCont.NewTuple();
                pCont.Current.ERPxContatoTipoID = ERPxContatoTipo.XDefault.Telefone_Fixo;
                pCont.Current.Contato = ctnr;
                pCont.Current.ERPxFinalidadeID = ERPxFinalidade.XDefault.Outros;
            }
            ctnr = GetValue(pData, "E_MAIL");
            if (ctnr.IsFull())
            {
                pCont.NewTuple();
                pCont.Current.ERPxContatoTipoID = ERPxContatoTipo.XDefault.EMail;
                pCont.Current.Contato = ctnr;
                pCont.Current.ERPxFinalidadeID = ERPxFinalidade.XDefault.Outros;
            }
            ctnr = GetValue(pData, "ENDERECO");
            String codibge = GetValue(pData, "CODIGO_MUNICIPIO");

            if (ctnr.IsFull())
            {
                pEnd.NewTuple();
                pEnd.Current.CEPxLogradouroID = pCEPxLogradouroID;
                pEnd.Current.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                pEnd.Current.ERPxFinalidadeID = ERPxFinalidade.XDefault.Outros;
            }
        }
    }
}