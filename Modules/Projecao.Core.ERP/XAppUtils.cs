using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

using static Projecao.Core.ERP.DB.ERPx;
using static TFX.Core.Service.Apps.SYSx;

namespace TFX.Core.Service.Apps
{
    public class XAppUtils
    {
        public static Guid InsertGrupoCPF(XExecContext pContext, XIBasePersistence pPromoteTable, String pCPF, Boolean pPromote = false)
        {
            using (_ERPxDocumento doc = XPersistencePool.Get<_ERPxDocumento>(pContext))
            using (_SYSxPessoaEmpresaGrupo peg = XPersistencePool.Get<_SYSxPessoaEmpresaGrupo>(pContext))
            {
                if (doc.Open(ERPxDocumento.Numero, pCPF, ERPxDocumento.ERPxDocumentoTipoID, ERPxDocumentoTipo.XDefault.CPF))
                {
                    if (!peg.Open(SYSxPessoaEmpresaGrupo.SYSxPessoaID, doc.Current.SYSxPessoaID, SYSxPessoaEmpresaGrupo.SYSxEmpresaGrupoID, pContext.Logon.CompanyGroupID))
                    {
                        peg.NewTuple();
                        peg.Current.SYSxEmpresaGrupoID = pContext.Logon.CompanyGroupID;
                        peg.Current.SYSxPessoaID = doc.Current.SYSxPessoaID;
                        peg.Flush();
                    }
                    if (!pPromoteTable.Open(doc.Current.SYSxPessoaID))
                    {
                        pPromoteTable.NewTuple(doc.Current.SYSxPessoaID);
                        XORMField state = pPromoteTable.Fields.First(f => f.Name == XDefault.StateFieldName);
                        if (state != null)
                            pPromoteTable.Current.Value[state] = SYSxEstado.XDefault.Ativo;
                        pPromoteTable.Flush();
                    }
                    doc.Current.IsPromoteOk = true;
                }
                else
                {
                    if (!pPromote)
                        throw new XUnconformity("Não é permitido Pessoas Física sem CPF.");
                    return Guid.Empty;
                }
                return doc.Current.SYSxPessoaID;
            }
        }

        public static Guid InsertGrupoCNPJ(XExecContext pContext, XIBasePersistence pPromoteTable, String pCNPJ, Boolean pPromote = false)
        {
            using (_ERPxDocumento doc = XPersistencePool.Get<_ERPxDocumento>(pContext))
            using (_SYSxPessoaEmpresaGrupo peg = XPersistencePool.Get<_SYSxPessoaEmpresaGrupo>(pContext))
            {
                if (doc.Open(ERPxDocumento.Numero, pCNPJ, ERPxDocumento.ERPxDocumentoTipoID, ERPxDocumentoTipo.XDefault.CNPJ))
                {
                    if (!peg.Open(SYSxPessoaEmpresaGrupo.SYSxPessoaID, doc.Current.SYSxPessoaID, SYSxPessoaEmpresaGrupo.SYSxEmpresaGrupoID, pContext.Logon.CompanyGroupID))
                    {
                        peg.NewTuple();
                        peg.Current.SYSxEmpresaGrupoID = pContext.Logon.CompanyGroupID;
                        peg.Current.SYSxPessoaID = doc.Current.SYSxPessoaID;
                        peg.Flush();
                    }
                    if (!pPromoteTable.Open(doc.Current.SYSxPessoaID))
                    {
                        pPromoteTable.NewTuple(doc.Current.SYSxPessoaID);
                        XORMField state = pPromoteTable.Fields.First(f => f.Name == XDefault.StateFieldName);
                        if (state != null)
                            pPromoteTable.Current.Value[state] = SYSxEstado.XDefault.Ativo;

                        pPromoteTable.Flush();
                    }
                    doc.Current.IsPromoteOk = true;
                }
                else
                {
                    if (!pPromote)
                        throw new XUnconformity("Não é permitido Pessoa Jurídca sem CNPJ.");
                    return Guid.Empty;
                }
                return doc.Current.SYSxPessoaID;
            }
        }
    }
}