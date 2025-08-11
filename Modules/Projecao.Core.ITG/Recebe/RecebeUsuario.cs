using System;
using System.IO;
using System.Reflection.PortableExecutable;

using iText.StyledXmlParser.Jsoup.Nodes;

using Projecao.Core.ERP.Pessoa;
using Projecao.Core.ERP.PessoaFisica;
using Projecao.Core.ERP.PessoaJuridica;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Apps.Usuario;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.CEP.DB.CEPx;
using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.CTLx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeUsuario), sCID)]
    public class RecebeUsuario : XClientMessageRule
    {
        private const String sCID = "72F830C8-2BC3-4643-A107-D484828F4B68";
        public static Guid gCID = new Guid(sCID);

        public override void Execute(XExecContext pContext)
        {
            using (XMemoryStream ms = new XMemoryStream(Data))
            using (StreamReader sr = new StreamReader(ms, XEncoding.Default))
            using (_SYSxEmpresa empresa = XPersistencePool.Get<_SYSxEmpresa>(pContext))
            using (UsuarioSVC.XService usuario = XServicePool.Get<UsuarioSVC.XService>(pContext))
            {
                Fields = sr.ReadLine().SafeBreak(";");
                while (!sr.EndOfStream)
                {
                    String[] data = sr.ReadLine().SafeBreak(";", StringSplitOptions.None);
                    if (data.IsEmpty())
                        continue;
                    try
                    {
                        String empnr = GetValue(data, "NUMERO_EMPRESA");
                        if (!empresa.Open(SYSxEmpresa.NumeroLegado, empnr))
                        {
                            XLog.Error(GetType(), $"Empresa com número [{empnr}] não contrada no banco.");
                            continue;
                        }
                        if (empresa.Current.SYSxEmpresaID != pContext.Logon.MasterCompanyID)
                            continue;
                        UsuarioSVC.XTuple utpl;
                        String unome = GetValue(data, "NOME");
                        if (usuario.Open(UsuarioSVC.xCTLxUsuario.Email, unome, UsuarioSVC.xCTLxUsuario.EmpresaLegadoID, empnr))
                            utpl = usuario.Current;
                        else
                            utpl = usuario.NewTuple();
                        utpl.Nome = unome;
                        utpl.Email = unome;
                        if (usuario.Current.State == XTupleState.New)
                        {
                            utpl.Senha = GetValue(data, "SENHA");
                            utpl.Integracao = true;
                            utpl.CodigoLegado = GetValue(data, "FUNCIONARIO");
                            utpl.EmpresaLegadoID = empnr;
                            utpl.SYSxEmpresaMatrizID = empresa.Current.SYSxEmpresaID;
                            utpl.SYSxEmpresaProprietariaID = empresa.Current.SYSxEmpresaID;
                            utpl.SuperUsuario = true;

                            UsuarioEmpresaSVC.XTuple uetpl = usuario.DataSet.UsuarioEmpresaDataSet.NewTuple();
                            uetpl.SYSxEmpresaPermitidaID = empresa.Current.SYSxEmpresaID;
                            uetpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;

                            UsuarioTemplateSVC.XTuple utepl = usuario.DataSet.UsuarioTemplateDataSet.NewTuple();
                            utepl.CTLxTemplateID = CTLxTemplate.XDefault.Vendedor;
                            utepl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                        }
                        usuario.Flush();
                    }
                    catch (Exception pEx)
                    {
                        Log.Error(pEx);
                    }
                }
            }
        }
    }
}