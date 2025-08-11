using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projecao.Core.ERP.Pessoa;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Apps.Tools;
using TFX.Core.Service.Apps.Usuario;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.ERP.DB.ERPx;
using static TFX.Core.Service.Apps.CTLx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ERP.Empresa
{
    [XRegister(typeof(EmpresaRule), sCID, typeof(EmpresaSVC))]
    public class EmpresaRule : EmpresaSVC.XRule
    {
        public const String sCID = "5A178641-4C39-4A22-9A69-222763FB1BD4";
        public static Guid gCID = new Guid(sCID);

        public EmpresaRule()
        {
            ID = gCID;
        }

        protected override void Prepare()
        {
            Model.CheckTenant = false;
        }

        protected override string ExecuteCustom(XExecContext pContext, EmpresaSVC.XDataSet pDataSet)
        {
            if (pContext.Broker.AuxData.SafeLength() != 1)
                throw new XError("Não foi encontrado aqruivo para importação");
            String[] lines = pContext.Broker.AuxData[0].ToString().SafeBreak(XEnvironment.NewLine);
            if (lines.SafeLength() == 0)
                throw new XUnconformity("Arquivo recebido não condiz com esperado.");
            List<String> header = XUtils.ParseCSV(lines[0], ';');
            if (header.Count != 6)
                throw new XUnconformity("Quantidade de colunas no aqrquivo deve ser 6.");
            EmpresaSVC.XService emp = GetService<EmpresaSVC.XService>();
            DocumentoSVC.XService docsvc = GetService<DocumentoSVC.XService>();
            Int32 cnt = 0;
            for (int i = 1; i < lines.SafeLength(); i++)
            {
                List<String> data = XUtils.ParseCSV(lines[i], ';');
                if (header.Count != 6)
                    throw new XUnconformity($"Quantidade de colunas na linha {i} do aqrquivo deve ser 6.");

                String cnpj = data[3].OnlyNumbers();
                if (emp.Open(EmpresaSVC.xERPxDocumento.Numero, cnpj))
                {
                    if (emp.Current.NumeroLegado.IsEmpty())
                    {
                        emp.Current.NumeroLegado = data[1];
                        emp.Flush();
                    }
                    continue;
                }
                cnt++;
                emp.Clear();
                EmpresaSVC.XTuple tpl = emp.NewTuple();
                if (docsvc.Open(DocumentoSVC.xERPxDocumento.Numero, cnpj))
                {
                    emp.DataSet.Current.NumeroLegado = data[1];
                    emp.DataSet.Current.CNPJ = cnpj;
                    emp.DataSet.Current.Sigla = data[2];                    
                    ExecutePromote(pContext, emp.DataSet);
                    continue;
                }
                tpl.NumeroLegado = data[1];
                tpl.Sigla = data[2];
                tpl.RazaoSocial = data[4];
                tpl.Nome = data[5];
                DocumentoSVC.XTuple doc = emp.DataSet.DocumentoDataSet.NewTuple();
                doc.SYSxPessoaID = tpl.SYSxPessoaID;
                doc.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                doc.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CNPJ;
                doc.Numero = cnpj;
                emp.Flush();
            }
            return $"Incluído {cnt} novas empresas.";
        }

        protected override void ExecutePromote(XExecContext pContext, EmpresaSVC.XDataSet pDataSet)
        {
            if (pDataSet?.Current.CNPJ.IsEmpty() == true)
                return;

            if (pDataSet?.Current.Sigla.IsEmpty() == true)
                throw new XUnconformity("Informe a Sigla antes.");

            if (!XUtils.CheckCNPJ(pDataSet.Current.CNPJ))
                throw new XUnconformity("CNPJ inválido.");

            _SYSxEmpresa emp = GetTable<_SYSxEmpresa>();
            _ERPxDocumento doc = GetTable<_ERPxDocumento>();
            
            if (doc.Open(ERPxDocumento.Numero, pDataSet?.Current.CNPJ.OnlyNumbers(), ERPxDocumento.ERPxDocumentoTipoID, ERPxDocumentoTipo.XDefault.CNPJ))
            {
                if (!emp.Open(doc.Current.SYSxPessoaID))
                {
                    emp.NewTuple(doc.Current.SYSxPessoaID);
                    emp.Current.Sigla = pDataSet?.Current.Sigla;
                    emp.Current.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                    emp.Flush();
                }
                pDataSet.Current.SYSxPessoaID = doc.Current.SYSxPessoaID;
                AddGrupo(pContext, pDataSet);
                pDataSet.Current.IsPromoteOk = true;
                pDataSet.Current.PKValue = doc.Current.SYSxPessoaID;
            }
            else
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.NewTuple();
                dtpl.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CNPJ;
                dtpl.Numero = pDataSet?.Current.CNPJ;
                dtpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                dtpl.Tipo = ERPxDocumentoTipo.XDefault.sCNPJ;
                dtpl.Mascara = EmpresaSVC.CNPJ.Mask;
            }
        }

        protected override void BeforeFlush(XExecContext pContext, EmpresaSVC pModel, EmpresaSVC.XDataSet pDataSet)
        {
            StringBuilder sb = new StringBuilder();

            foreach (EmpresaSVC.XTuple etpl in pDataSet)
            {
                if (etpl.State != XTupleState.New)
                {
                    foreach (DocumentoSVC.XTuple tpl in pDataSet.DocumentoDataSet.Tuples.Where(t => t.SYSxPessoaID == etpl.SYSxPessoaID))
                    {
                        if (tpl.State == XTupleState.Changed && tpl.oERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CNPJ)
                            sb.AppendLine($"Empresa '{etpl.Nome}' foi alterado CNPJ, não é permitido.");
                    }
                }
                else
                {
                }
            }
            if (sb.Length > 0)
                throw new XUnconformity(sb.ToString());
        }

        protected override void AfterFlush(XExecContext pContext, EmpresaSVC pModel, EmpresaSVC.XDataSet pDataSet)
        {
            AddGrupo(pContext, pDataSet);
        }

        private void AddGrupo(XExecContext pContext, EmpresaSVC.XDataSet pDataSet)
        {
            UsuarioSVC.XService susr = GetService<UsuarioSVC.XService>();
            _SYSxEmpresaGrupo grp = GetTable<_SYSxEmpresaGrupo>();
            _SYSxEmpresa emp = GetTable<_SYSxEmpresa>();

            _SYSxPessoaEmpresaGrupo pesgrp = GetTable<_SYSxPessoaEmpresaGrupo>();

            foreach (EmpresaSVC.XTuple etpl in pDataSet.Where(t => t.State == XTupleState.New))
            {
                pesgrp.Clear();
                grp.Clear();
                SYSx.SYSxEmpresaGrupo.XTuple gtpl;
                if (!grp.Open(SYSx.SYSxEmpresaGrupo.Grupo, etpl.Sigla))
                {
                    gtpl = grp.NewTuple();
                    gtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
                    gtpl.SYSxPrincipalEmpresaID = etpl.SYSxPessoaID;
                    gtpl.Grupo = etpl.Sigla;
                }
                else
                    gtpl = grp.Current;
                SYSx.SYSxPessoaEmpresaGrupo.XTuple petpl;
                if (!pesgrp.Open(SYSx.SYSxPessoaEmpresaGrupo.SYSxPessoaID, etpl.SYSxPessoaID, SYSx.SYSxPessoaEmpresaGrupo.SYSxEmpresaGrupoID, gtpl.SYSxEmpresaGrupoID))
                {
                    petpl = pesgrp.NewTuple(Guid.NewGuid());
                    petpl.SYSxPessoaID = etpl.SYSxPessoaID;
                    petpl.SYSxEmpresaGrupoID = gtpl.SYSxEmpresaGrupoID;
                }

                emp.Open(etpl.SYSxPessoaID);
                emp.Current.SYSxEmpresaMatrizID = etpl.SYSxPessoaID;
                emp.Current.SYSxEmpresaGrupoID = gtpl.SYSxEmpresaGrupoID;

                grp.Flush();
                pesgrp.Flush();
                emp.Flush();

                AddUsuario(pContext, susr, etpl.SYSxPessoaID, $"Admin para {etpl.Sigla}", $"{etpl.Sigla.ToLower()}@gestpet.com.br");
                susr.Flush();
                AtualizaTemplate.Atualiza(pContext, susr.DataSet);
            }
        }

        //protected override String ExecuteCustom(XExecContext pContext, EmpresaSVC.XDataSet pDataSet)
        //{
        //    XCustomerKey key = XCustomerKey.Deserialize((String)pContext.Broker.AuxData.First());
        //    _ERPxPessoaJuridica pj = GetTable<_ERPxPessoaJuridica>();
        //    _ERPxDocumento doc = GetTable<_ERPxDocumento>();
        //    _SYSxEmpresaGrupo grp = GetTable<_SYSxEmpresaGrupo>();
        //    _SYSxPessoa pes = GetTable<_SYSxPessoa>();
        //    _SYSxPessoaEmpresaGrupo pesgrp = GetTable<_SYSxPessoaEmpresaGrupo>();
        //    _SYSxEmpresa emp = GetTable<_SYSxEmpresa>();
        //    UsuarioSVC.XService susr = GetService<UsuarioSVC.XService>();
        //    if (pes.Open(key.ID))
        //        throw new XUnconformity($"Empresa '{key.Nome}' já foi incluida.");

        //    ERPx.ERPxPessoaJuridica.XTuple pjtpl = pj.NewTuple(key.ID);
        //    pjtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
        //    pjtpl.RazaoSocial = key.RazaoSocial;

        //    ERPx.ERPxDocumento.XTuple dctpl = doc.NewTuple(key.ERPxDocumentoID);
        //    dctpl.SYSxPessoaID = key.ID;
        //    dctpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
        //    dctpl.ERPxDocumentoTipoID = ERPx.ERPxDocumentoTipo.XDefault.CNPJ;
        //    dctpl.Numero = key.CNPJ;

        //    SYSx.SYSxPessoa.XTuple ptpl = pes.NewTuple(key.ID);
        //    ptpl.Nome = key.Nome;

        //    SYSx.SYSxEmpresaGrupo.XTuple gtpl = grp.NewTuple(key.SYSxGrupoID);
        //    gtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
        //    gtpl.SYSxPrincipalEmpresaID = key.SYSxEmpresaMatrizID;
        //    gtpl.Grupo = key.Grupo;

        //    SYSx.SYSxPessoaEmpresaGrupo.XTuple petpl = pesgrp.NewTuple(Guid.NewGuid());
        //    petpl.SYSxPessoaID = key.ID;
        //    petpl.SYSxEmpresaGrupoID = key.SYSxGrupoID;

        //    SYSx.SYSxEmpresa.XTuple etpl = emp.NewTuple(key.ID);
        //    etpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
        //    etpl.SYSxEmpresaGrupoID = key.SYSxGrupoID;
        //    etpl.SYSxEmpresaMatrizID = key.SYSxEmpresaMatrizID;
        //    etpl.Sigla = key.Sigla;

        //    AddUsuario(pContext, susr, key.ID, $"Admin para {key.Sigla}", $"{key.Sigla.ToLower()}@gestpet.com.br");

        //    pes.Flush();
        //    pj.Flush();
        //    grp.Flush();
        //    emp.Flush();
        //    doc.Flush();
        //    pesgrp.Flush();
        //    susr.Flush();
        //    AtualizaTemplate.Atualiza(pContext, susr.DataSet);
        //    return key.ID.AsString();
        //}

        private static void AddUsuario(XExecContext pContext, UsuarioSVC.XService pUsuario, Guid pEmpresaID, String pUsuarioNome, String pEMail)
        {
            if (pEMail.IsFull())
            {
                UsuarioSVC.XTuple utpl = pUsuario.NewTuple();
                utpl.Nome = pUsuarioNome;
                utpl.Email = pEMail;
                utpl.SYSxEmpresaMatrizID = pEmpresaID;
                utpl.SYSxEmpresaProprietariaID = pContext.Logon.MasterCompanyID;
                utpl.SuperUsuario = true;

                UsuarioEmpresaSVC.XTuple uetpl = pUsuario.DataSet.UsuarioEmpresaDataSet.NewTuple();
                uetpl.SYSxEmpresaPermitidaID = pEmpresaID;
                uetpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;

                UsuarioTemplateSVC.XTuple utepl = pUsuario.DataSet.UsuarioTemplateDataSet.NewTuple();
                utepl.CTLxTemplateID = CTLxTemplate.XDefault.Administrador_de_Loja;
                utepl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            }
        }

        protected override void AfterGet(XExecContext pContext, EmpresaSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.DocumentoDataSet.Tuples.Where(t => t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CNPJ).ForEach(t => t.IsReadOnly = true);
        }
    }
}