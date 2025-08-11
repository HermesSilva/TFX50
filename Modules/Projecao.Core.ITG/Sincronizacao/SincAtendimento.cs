using System;
using System.Linq;
using System.Text;

using Projecao.Core.ERP.DB;
using Projecao.Core.ERP.Pessoa;
using Projecao.Core.ERP.PessoaFisica;
using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Envio;
using Projecao.Core.LGC.DB;
using Projecao.Core.PET.Agendamento;
using Projecao.Core.PET.Tutor;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Utils;

using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(SincAtendimento), sCID)]
    public class SincAtendimento : XSincronize
    {
        public const String sCID = "BC957C9B-9A19-4EAB-9EA7-DBA7D5C6D9E9";
        public static Guid gCID = new Guid(sCID);
        private static Boolean _Initialized;

        public SincAtendimento()
        {
        }

        private XDBLegacyTable _Stq;
        private XDBLegacyTable _Stqlt;
        private Boolean _UsaGen = true;

        private String _CNPJFMT;
        private Boolean _FechaPedido = false;

        private Boolean _BaixEstoque = true;
        public override Int32 RunnOrder => 40;

        public override void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            if (!_Initialized)
            {
                try
                {
                    PedidoTable ped = new PedidoTable();
                    ped.Configure(pContext);
                    ped.Prepare(pContext, null);
                    ped.Execute(pContext);
                    ped.Prepare(null, null);
                    ItemPedidoTable iped = new ItemPedidoTable();
                    iped.Configure(pContext);
                    iped.Prepare(pContext, null);
                    iped.Execute(pContext);
                    iped.Prepare(null, null);
                }
                catch (Exception pEx)
                {
                    Log.Error(pEx);
                }
                finally
                {
                    _Initialized = true;
                }
            }
            _FechaPedido = Config.FechaPedido;
            using (AgendamentoSVC.XService atend = XServicePool.Get<AgendamentoSVC.XService>(AgendamentoSVC.gCID))
            using (TutorSVC.XService pessoa = XServicePool.Get<TutorSVC.XService>(TutorSVC.gCID))
            using (XDBLegacyTable vend = XLegacyManager.Create(XGerenciaIntegracao.FAT_FUNCIONARIOS, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable pedido = XLegacyManager.Create(XGerenciaIntegracao.FAT_CAPAPEDIDO, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable itpedido = XLegacyManager.Create(XGerenciaIntegracao.FAT_ITEMPEDIDO, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable prod = XLegacyManager.Create(XGerenciaIntegracao.FAT_PRODUTOS, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable cad = XLegacyManager.Create(XGerenciaIntegracao.FAT_CADASTROS, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable cid = XLegacyManager.Create(XGerenciaIntegracao.FAT_CIDADES, pContext, XGerenciaIntegracao.KnowTables, true))
            using (XDBLegacyTable stq = _Stq = XLegacyManager.Create(XGerenciaIntegracao.FAT_ESTOQUE, pContext, XGerenciaIntegracao.KnowTables))
            using (XDBLegacyTable stqlt = _Stqlt = XLegacyManager.Create(XGerenciaIntegracao.FAT_ESTOQUELOTE, pContext, XGerenciaIntegracao.KnowTables))
            {
                ConcluiAtendimentoSVC mdl = new ConcluiAtendimentoSVC();
                mdl.Load();
                ConcluiAtendimentoSVC.XDataSet dstcc = mdl.PrepareDataSet<ConcluiAtendimentoSVC.XDataSet>();

                try
                {
                    using (XQuery qry = new XQuery(pContext))
                    {
                        qry.SQL = "SELECT STATUS,TOTAL_PEDIDO,ID_PEDIDO_PET FROM FAT_CAPAPEDIDO WHERE ID_PEDIDO_PET IN (SELECT DISTINCT(CHAVE) FROM ITG_DELECAO WHERE TABELA  IN ('FAT_CAPAPEDIDO','FAT_ITEMPEDIDO' ))";
                        if (qry.Open())
                        {
                            using (XExecContext ctx = pContext.Clone())
                            {
                                foreach (XDataTuple tpl in qry.Tuples.Where(t => t["STATUS"].As<String>() == "CAN"))
                                {
                                    dstcc.Clear();
                                    dstcc.NewTuple(new Guid(tpl["ID_PEDIDO_PET"].As<String>()));
                                    dstcc.Current.EstadoID = PETxAtendimentoEstado.XDefault.Cancelado;
                                    ctx.DataBase.ExecSQL($"delete from ITG_DELECAO where CHAVE = '{tpl["ID_PEDIDO_PET"].As<String>()}'");
                                }
                                if (pBroker.OnlyPut(dstcc))
                                    ctx.Commit();
                                else
                                    ctx.Rollback();
                            }
                            //using (XExecContext ctx = pContext.Clone())
                            //{
                            //    foreach (XDataTuple tpl in qry.Tuples.Where(t => t["STATUS"].As<String>() != "CAN"))
                            //    {
                            //        dstcc.Clear();
                            //        dstcc.NewTuple(new Guid(tpl["ID_PEDIDO_PET"].As<String>()));
                            //        dstcc.Current.EstadoID = PETxAtendimentoEstado.XDefault.Cancelado;
                            //        ctx.DataBase.ExecSQL($"delete from ITG_DELECAO where CHAVE = '{tpl["ID_PEDIDO_PET"].As<String>()}'");
                            //    }
                            //    if (pBroker.OnlyPut(dstcc))
                            //        ctx.Commit();
                            //    else
                            //        ctx.Rollback();
                            //}
                        }
                    }
                }
                catch (Exception pEx)
                {
                    Log.Error(pEx);
                }

                _CNPJFMT = pContext.DataBase.ExecScalar<String>("SELECT EMPRESA_ID FROM FAT_PARAMETROS");
                _UsaGen = pContext.DataBase.ExecScalar<String>($"select CONTROLE_SEQUENCIA from FAT_PARAMETROS WHERE EMPRESA_ID = '{_CNPJFMT}'") != "TABELA";

                XSearchData sdata = XHttpRequest.GetWhere(PETxAtendimentoEstado.XDefault.Fechado.AsString(), AgendamentoSVC.xPETxAtendimento.PETxAtendimentoEstadoID);
                AgendamentoSVC.XDataSet atenddst = pBroker.Get<AgendamentoSVC, AgendamentoSVC.XDataSet>(sdata, 0);
                //atenddst.ShowData();
                foreach (AgendamentoSVC.XTuple tpl in atenddst)
                {
                    try
                    {
                        using (XExecContext ctx = pContext.Clone())
                        {
                            ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                            sdata = XHttpRequest.GetWhere(tpl.PETxTutorID.AsString(), PessoaFisicaSVC.xSYSxPessoa.SYSxPessoaID);
                            PessoaFisicaSVC.XDataSet dstpf = pBroker.Get<PessoaFisicaSVC, PessoaFisicaSVC.XDataSet>(sdata, 0);
                            //dstpf.ShowData();
                            StringBuilder sb = new StringBuilder();

                            ValidaUsuarioProduto(vend, prod, atenddst, tpl, sb);
                            if (sb.Length > 0)
                                throw new Exception(sb.ToString());

                            Decimal descpacote = atenddst.AgendamentoVacinaDataSet.Tuples.Where(t => tpl.PETxAtendimentoID == t.PETxAtendimentoID).Sum(t => t.DescontoPacote) +
                                                 atenddst.AgendamentoServicoDataSet.Tuples.Where(t => tpl.PETxAtendimentoID == t.PETxAtendimentoID).Sum(t => t.DescontoPacote);
                            Int32 codcli = 0;
                            foreach (DocumentoSVC.XTuple pestpl in dstpf.DocumentoDataSet.Tuples.Where(d => d.ERPxDocumentoTipoID == ERPx.ERPxDocumentoTipo.XDefault.CPF))
                            {
                                String doc = XUtils.FormatCPF(pestpl.Numero);

                                codcli = ctx.DataBase.ExecScalar<Int32>("select CODIGO_EXP from FAT_CADASTROS", "CAD_CGC", doc);
                                if (codcli == 0)
                                    codcli = InserePessoa(ctx, ctx, cad, cid, dstpf);
                            }

                            String pedpk = InserePedido(ctx, pedido, tpl, codcli, tpl.CodigoLegado, descpacote, tpl.ValorCobrado - descpacote);
                            InsereItemPedido(itpedido, atenddst, tpl.PETxAtendimentoID, pedpk, tpl.CodigoLegado, 1);

                            dstcc.Clear();
                            dstcc.NewTuple(tpl.PETxAtendimentoID);
                            dstcc.Current.EstadoID = PETxAtendimentoEstado.XDefault.Recebido;
                            if (pBroker.OnlyPut(dstcc))
                                ctx.Commit();
                            else
                                ctx.Rollback();
                        }
                    }
                    catch (Exception pEx)
                    {
                        Log.Error(pEx);
                        continue;
                    }
                }
            }
        }

        private Int32 InsereItemPedido(XDBLegacyTable pItem, AgendamentoSVC.XDataSet pAtendimento, Guid pAgedamentoID, String pPedPK, String pUsuarioID, Int32 pItemCount)
        {
            foreach (AgendamentoServicoSVC.XTuple svctpl in pAtendimento.AgendamentoServicoDataSet.Tuples.Where(t => t.PETxAtendimentoID == pAgedamentoID))
            {
                pItem.Clear();
                pItemCount = AddCommuns(pItem, pPedPK, pUsuarioID, pItemCount, pAgedamentoID);
                AddServico(pItem, svctpl);
                _BaixEstoque = svctpl.DescontoPacote == 0;
                BaixaEstoque(svctpl.ProdutoID, svctpl.Quantidade);
                pItem.Flush();
            }
            foreach (AgendamentoExamesSVC.XTuple svctpl in pAtendimento.AgendamentoExamesDataSet.Tuples.Where(t => t.PETxAtendimentoID == pAgedamentoID))
            {
                pItem.Clear();
                pItemCount = AddCommuns(pItem, pPedPK, pUsuarioID, pItemCount, pAgedamentoID);
                AddExame(pItem, svctpl);
                _BaixEstoque = true;
                BaixaEstoque(svctpl.ProdutoID, 0);
                pItem.Flush();
            }
            foreach (AgendamentoVacinaSVC.XTuple svctpl in pAtendimento.AgendamentoVacinaDataSet.Tuples.Where(t => t.PETxAtendimentoID == pAgedamentoID))
            {
                pItem.Clear();
                pItemCount = AddCommuns(pItem, pPedPK, pUsuarioID, pItemCount, pAgedamentoID);
                AddVacina(pItem, svctpl);
                _BaixEstoque = svctpl.DescontoPacote == 0;
                BaixaEstoque(svctpl.ProdutoID, svctpl.Quantidade);
                pItem.Flush();
            }

            foreach (AgendamentoProdutoSVC.XTuple svctpl in pAtendimento.AgendamentoProdutoDataSet.Tuples.Where(t => t.PETxAtendimentoID == pAgedamentoID))
            {
                pItem.Clear();
                pItemCount = AddCommuns(pItem, pPedPK, pUsuarioID, pItemCount, pAgedamentoID);
                AddAcessorio(pItem, svctpl);
                _BaixEstoque = true;
                BaixaEstoque(svctpl.ProdutoID, svctpl.Quantidade);
                pItem.Flush();
            }
            return pItemCount;
        }

        private Int32 InserePessoa(XExecContext pContext, XExecContext pLocalContext, XDBLegacyTable pPessoa, XDBLegacyTable pCidade, PessoaFisicaSVC.XDataSet pOrigem)
        {
            PessoaFisicaSVC.XTuple pestpl = pOrigem.Current;
            EnderecoSVC.XTuple endtpl = pOrigem.EnderecoDataSet.Tuples.FirstOrDefault();
            if (endtpl == null)
                throw new XUnconformity($"A Pessoa [{pestpl.Nome}] não possui endereço, não poderá ser integrado os dados.");

            ContatoSVC.XTuple emailtpl = pOrigem.ContatoDataSet.Tuples.FirstOrDefault(t => t.ERPxContatoTipoID == ERPxContatoTipo.XDefault.EMail);
            ContatoSVC.XTuple fonetpl = pOrigem.ContatoDataSet.Tuples.FirstOrDefault(t => t.ERPxContatoTipoID.In(ERPxContatoTipo.XDefault.Telefone_Celular, ERPxContatoTipo.XDefault.Telefone_Fixo));
            DocumentoSVC.XTuple doctpl = pOrigem.DocumentoDataSet.Tuples.FirstOrDefault(t => t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF);
            String cidpk = "";
            cidpk = BuscaCidade(pContext, pCidade, endtpl);

            Int32 pk = 0;
            Int32 cnt = 0;
            do
            {
                pk = pContext.DataBase.ExecScalar<Int32>("SELECT FAT_SEQ_CLIENTES.NEXTVAL FROM DUAL");
                cnt++;
            }
            while (pContext.DataBase.ExecScalar<Int32>("SELECT count(*) from FAT_CADASTROS  where CODIGO_EXP = " + pk) > 0 && cnt < 1000);

            pPessoa.Clear();
            pPessoa.NewTuple(pk);
            pPessoa.Current["CIDADE"] = cidpk;
            pPessoa.Current["CODIGO_GRUPO"] = "00001";
            pPessoa.Current["CODIGO_SETOR"] = "00001";
            pPessoa.Current["CONVENIO"] = 0;
            pPessoa.Current["CODIGO_ROTA"] = "001.001";
            pPessoa.Current["TABELA_PRECO"] = "1";
            pPessoa.Current["RETENCAO2"] = "N";
            pPessoa.Current["RETENCAO1"] = "N";
            pPessoa.Current["MAXIMO_PERCENTUAL"] = 0;
            pPessoa.Current["FATURA_AVISTA"] = "N";
            pPessoa.Current["ESTIMATIVA"] = "S";
            pPessoa.Current["CONSUMIDOR_FINAL"] = "S";
            pPessoa.Current["NOME_FANTAZIA"] = pPessoa.Current["RAZAO_SOCIAL"] = pestpl.Nome.CleanString().SafeUpper().SafePad(40);

            pPessoa.Current["CIDADE_ENT"] = cidpk;
            pPessoa.Current["CIDADE_FIN"] = cidpk;
            pPessoa.Current["NATUREZA_OPERACAO_DEFAULT"] = null;
            pPessoa.Current["CODIGO_PRZ"] = null;
            pPessoa.Current["CODIGO_BANCO"] = "9999";
            pPessoa.Current["CODIGO_BANCO2"] = null;

            if (fonetpl != null)
                pPessoa.Current["TELEFONE"] = pPessoa.Current["FAX"] = pPessoa.Current["CELULAR"] = fonetpl.Contato;
            if (emailtpl != null)
                pPessoa.Current["E_MAIL"] = emailtpl.Contato;
            pPessoa.Current["CODIGO_EXP"] = pk;
            pPessoa.Current["CAD_CGC"] = XUtils.FormatCPF(doctpl.Numero);
            pPessoa.Current["TIPO"] = "N";
            pPessoa.Current["CALCULAR_COMISSAO"] = "S";
            pPessoa.Current["CLIENTE_BLOQUEADO"] = "N";
            pPessoa.Current["CGC_TRAN"] = _CNPJFMT;
            pPessoa.Current["DATA_HORA_ALTERACAO"] = DateTime.Now;
            pPessoa.Current["EMPRESA_ALTERACAO"] = _CNPJFMT;
            pPessoa.Current["DATA_CADASTRO"] = DateTime.Now;
            pPessoa.Flush();
            return pk;
        }

        private String InserePedido(XExecContext pConext, XDBLegacyTable pPedido, AgendamentoSVC.XTuple pAtendimento, Int32 CodCli, String pUsuarioID, Decimal pDescPacote, Decimal pTotal)
        {
            pPedido.Clear();
            pPedido.NewTuple(pPedido.Count + 1);
            String pk;
            if (_UsaGen)
                pk = pConext.DataBase.ExecScalar<Int32>("select FAT_SEQ_PEDIDO.NEXTVAL from dual").ToString("0000000");
            else
            {
                pConext.DataBase.ExecSQL($"SELECT SEQUENCIA FROM FAT_SEQUENCIA WHERE EMPRESA_ID = '{_CNPJFMT}' AND NOME = 'FAT_SEQ_PEDIDO' FOR UPDATE");
                pConext.DataBase.ExecSQL($"UPDATE FAT_SEQUENCIA SET SEQUENCIA=SEQUENCIA+1 WHERE EMPRESA_ID = '{_CNPJFMT}' AND NOME = 'FAT_SEQ_PEDIDO'");
                pk = pConext.DataBase.ExecScalar<Int32>($"SELECT SEQUENCIA FROM FAT_SEQUENCIA WHERE EMPRESA_ID = '{_CNPJFMT}' AND NOME = 'FAT_SEQ_PEDIDO'").ToString("0000000");
            }
            pPedido.Current["ID_PEDIDO_PET"] = pAtendimento.PETxAtendimentoID.AsString();
            pPedido.Current["EMPRESA_ID"] = _CNPJFMT;
            pPedido.Current["PEDIDO"] = pk;
            pPedido.Current["CODIGO_EXP"] = CodCli;
            pPedido.Current["CGC_TRAN"] = _CNPJFMT;
            pPedido.Current["CODIGO_BANCO"] = "9999";
            pPedido.Current["CODIGO_PRZ"] = "00001";
            pPedido.Current["VENDEDOR"] = pUsuarioID;
            pPedido.Current["OPERADOR"] = pUsuarioID;
            pPedido.Current["DATA_PEDIDO"] = pAtendimento.Data;
            pPedido.Current["LISTOU_SEPARACAO"] = "N";
            pPedido.Current["OBSERVACAO"] = "Atendimento PET Número = " + pAtendimento.Numero;
            pPedido.Current["OBSERVACAO2"] = "";
            pPedido.Current["PERCENTUAL"] = 100;
            pPedido.Current["TIPO_PERCENTUAL"] = "Q";
            pPedido.Current["TIPO_FRETE"] = "FOB";
            if (_FechaPedido)
                pPedido.Current["STATUS"] = "FEC";
            else
                pPedido.Current["STATUS"] = "ORC";
            pPedido.Current["TOTAL_PEDIDO"] = pTotal;
            pPedido.Current["VALOR_FRETE"] = 0;
            pPedido.Current["NATUREZA_OPERACAO_ID"] = pConext.DataBase.ExecScalar<Int32>("SELECT NATUREZAOP_VENDA FROM FAT_PARAMETROS");
            pPedido.Current["DECIMAIS"] = 2;
            pPedido.Current["DECIMAIS_PRECO"] = 2;
            pPedido.Current["DESCONTO"] = 0;
            pPedido.Current["CALCULAR_ICMS"] = "S";
            pPedido.Current["ESPECIAL"] = "N";
            pPedido.Current["ENTREGA"] = "N";
            pPedido.Current["TIPOVENDA_ID"] = "00001";
            pPedido.Current["TIPO_MOV_ESTOQUE"] = "S";
            pPedido.Current["DATA_HORA_DIGITACAO"] = DateTime.Now;
            pPedido.Current["USUARIO_ALTERACAO"] = pUsuarioID;
            pPedido.Current["EMPRESA_ALTERACAO"] = _CNPJFMT;
            pPedido.Current["DATA_HORA_ALTERACAO"] = DateTime.Now;
            pPedido.Flush();
            return pk;
        }

        private static void ValidaUsuarioProduto(XDBLegacyTable vend, XDBLegacyTable prod, AgendamentoSVC.XDataSet dst, AgendamentoSVC.XTuple tpl, StringBuilder sb)
        {
            if (!vend.ExistsInDB("FUNCIONARIO", tpl.CodigoLegado))
                sb.AppendLineEx($"Usuário com ID=[{tpl.Email}] e código=[{tpl.CodigoLegado}] não foi encontrado na base de dados da Loja.");
            foreach (AgendamentoExamesSVC.XTuple exatpl in dst.AgendamentoExamesDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
            {
                if (!prod.ExistsInDB("CODIGO_PRO", exatpl.ProdutoID))
                    sb.AppendLineEx($"Produto Nome [{exatpl.Nome}] e código=[{exatpl.ProdutoID}] não foi encontrado na base de dados da Loja.");
            }
            foreach (AgendamentoProdutoSVC.XTuple exatpl in dst.AgendamentoProdutoDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
            {
                if (!prod.ExistsInDB("CODIGO_PRO", exatpl.ProdutoID))
                    sb.AppendLineEx($"Produto Nome [{exatpl.Nome}] e código=[{exatpl.ProdutoID}] não foi encontrado na base de dados da Loja.");
            }
            foreach (AgendamentoServicoSVC.XTuple exatpl in dst.AgendamentoServicoDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
            {
                if (!prod.ExistsInDB("CODIGO_PRO", exatpl.ProdutoID))
                    sb.AppendLineEx($"Produto Nome [{exatpl.Nome}] e código=[{exatpl.ProdutoID}] não foi encontrado na base de dados da Loja.");
            }
            foreach (AgendamentoVacinaSVC.XTuple exatpl in dst.AgendamentoVacinaDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
            {
                if (!prod.ExistsInDB("CODIGO_PRO", exatpl.ProdutoID))
                    sb.AppendLineEx($"Produto Nome [{exatpl.Nome}] e código=[{exatpl.ProdutoID}] não foi encontrado na base de dados da Loja.");
            }
        }

        private void BaixaEstoque(String pProdutoID, Decimal pQtde)
        {
            if (!_FechaPedido || !_BaixEstoque)
                return;
            if (_Stq.Open("EMPRESA_ID", _CNPJFMT, "CODIGO_PRO", pProdutoID))
            {
                _Stq.Current["QUANTIDADE"] = Convert.ToDecimal(_Stq.Current["QUANTIDADE"].ToString()) - pQtde;
                _Stq.Flush();
            }
            if (_Stqlt.Open("EMPRESA_ID", _CNPJFMT, "CODIGO_PRO", pProdutoID))
            {
                XDataTuple tpl = _Stqlt.Tuples.OrderBy(t => t["VENCIMENTO"]).First();
                tpl["QUANTIDADE"] = Convert.ToDecimal(tpl["QUANTIDADE"].ToString()) - pQtde;
                _Stqlt.Flush();
            }
        }

        private void AddAcessorio(XDBLegacyTable pItem, AgendamentoProdutoSVC.XTuple pTuple)
        {
            pItem.Current["ID_PEDIDO_PET"] = pTuple.PETxAtendimentoID.AsString();
            pItem.Current["CODIGO_PRO"] = pTuple.ProdutoID;
            if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
                pItem.Current["DESCONTO"] = pTuple.ValorTabela - pTuple.ValorCobrado;
            pItem.Current["PRECO_TABELA"] = pTuple.ValorTabela;
            pItem.Current["PRECO_VENDA"] = pTuple.ValorTotal / pTuple.Quantidade;
            pItem.Current["PRECO"] = pTuple.ValorTotal / pTuple.Quantidade;
            pItem.Current["DESCONTO_PADRAO"] = 0;
            pItem.Current["QUANTIDADE"] = pTuple.Quantidade;
        }

        private void AddExame(XDBLegacyTable pItem, AgendamentoExamesSVC.XTuple pTuple)
        {
            pItem.Current["ID_PEDIDO_PET"] = pTuple.PETxAtendimentoID.AsString();
            pItem.Current["CODIGO_PRO"] = pTuple.ProdutoID;
            if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
                pItem.Current["DESCONTO"] = pTuple.ValorTabela - pTuple.ValorCobrado;
            pItem.Current["PRECO_TABELA"] = pTuple.ValorTabela;
            pItem.Current["PRECO_VENDA"] = pTuple.ValorTabela;
            pItem.Current["PRECO"] = pTuple.ValorCobrado;
            pItem.Current["DESCONTO_PADRAO"] = 0;
            pItem.Current["QUANTIDADE"] = 1;
        }

        private void AddVacina(XDBLegacyTable pItem, AgendamentoVacinaSVC.XTuple pTuple)
        {
            pItem.Current["ID_PEDIDO_PET"] = pTuple.PETxAtendimentoID.AsString();
            pItem.Current["CODIGO_PRO"] = pTuple.ProdutoID;
            if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
            {
                pItem.Current["DESCONTO"] = 0;
                pItem.Current["PRECO_TABELA"] = 0.01M;
                pItem.Current["PRECO_VENDA"] = 0.01M;
                pItem.Current["PRECO"] = 0.01M;
            }
            else
            {
                if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
                    pItem.Current["DESCONTO"] = pTuple.ValorTabela - pTuple.ValorCobrado;
                pItem.Current["PRECO_TABELA"] = pTuple.ValorTabela;
                pItem.Current["PRECO_VENDA"] = pTuple.ValorTotal / pTuple.Quantidade;
                pItem.Current["PRECO"] = pTuple.ValorTotal / pTuple.Quantidade;
            }
            pItem.Current["DESCONTO_PADRAO"] = 0;
            pItem.Current["QUANTIDADE"] = pTuple.Quantidade;
        }

        private static void AddServico(XDBLegacyTable pItem, AgendamentoServicoSVC.XTuple pTuple)
        {
            pItem.Current["ID_PEDIDO_PET"] = pTuple.PETxAtendimentoID.AsString();
            pItem.Current["CODIGO_PRO"] = pTuple.ProdutoID;
            if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
                pItem.Current["DESCONTO"] = pTuple.ValorTabela - pTuple.ValorCobrado;
            if (pTuple.DescontoPacote > 0)
            {
                pItem.Current["DESCONTO"] = 0;
                pItem.Current["PRECO_TABELA"] = 0.01M;
                pItem.Current["PRECO_VENDA"] = 0.01M;
                pItem.Current["PRECO"] = 0.01M;
            }
            else
            {
                if (pTuple.ValorTabela - pTuple.ValorCobrado > 0)
                    pItem.Current["DESCONTO"] = pTuple.ValorTabela - pTuple.ValorCobrado;
                pItem.Current["PRECO_TABELA"] = pTuple.ValorTabela;
                pItem.Current["PRECO_VENDA"] = pTuple.ValorTotal / pTuple.Quantidade;
                pItem.Current["PRECO"] = pTuple.ValorTotal / pTuple.Quantidade;
            }
            pItem.Current["DESCONTO_PADRAO"] = 0;
            pItem.Current["QUANTIDADE"] = pTuple.Quantidade;
        }

        private Int32 AddCommuns(XDBLegacyTable pItem, String pPedPK, String pUsuarioID, Int32 cnt, Guid pAtendimentoID)
        {
            pItem.NewTuple(pItem.Count + 1);
            pItem.Current["EMPRESA_ID"] = _CNPJFMT;
            pItem.Current["ID_PEDIDO_PET"] = pAtendimentoID.AsString();
            pItem.Current["PEDIDO"] = pPedPK;
            pItem.Current["ITEM"] = cnt++;
            pItem.Current["LOTE"] = "X";
            pItem.Current["VENDEDOR"] = pUsuarioID;
            pItem.Current["UNIDADE"] = "UN";
            pItem.Current["FRACAO"] = 1;
            pItem.Current["ALTEROUPRECO"] = "S";
            pItem.Current["TIPOVENDA_ID"] = "0001";
            pItem.Current["USUARIO_ALTERACAO"] = pUsuarioID;
            pItem.Current["EMPRESA_ALTERACAO"] = _CNPJFMT;
            pItem.Current["DATA_HORA_ALTERACAO"] = DateTime.Now;
            return cnt;
        }

        private String BuscaCidade(XExecContext pConext, XDBLegacyTable pCidade, EnderecoSVC.XTuple pEndereco)
        {
            using (XDBLegacyTable uf = XLegacyManager.Create(XGerenciaIntegracao.FAT_ESTADOS, pConext, XGerenciaIntegracao.KnowTables, true))
            {
                if (!uf.ExistsInDB("ESTADO", pEndereco.Sigla.SafeUpper()))
                {
                    uf.NewTuple(pEndereco.Sigla.SafeUpper());
                    uf.Current["ESTADO"] = pEndereco.Sigla.SafeUpper();
                    uf.Current["NOME"] = pEndereco.NomeUF.SafeUpper();
                    uf.Current["DATA_HORA_ALTERACAO"] = DateTime.Now;
                    uf.Current["EMPRESA_ALTERACAO"] = _CNPJFMT;
                    uf.Current["COD_PAIS"] = "01058";
                    uf.Flush();
                }
                String cidpk;
                if (!pCidade.Open("CODIGO_MUNICIPIO", pEndereco.CodigoIBGE))
                {
                    Int32 cnt = 0;
                    do
                    {
                        cidpk = pConext.DataBase.ExecScalar<Int32>("SELECT FAT_SEQ_CIDADE.NEXTVAL FROM DUAL").ToString("00000");
                        cnt++;
                    }
                    while (pCidade.Open("CIDADE", cidpk) && cnt < 1000);

                    pCidade.Clear();
                    pCidade.NewTuple(cidpk);
                    pCidade.Current["EMPRESA_ALTERACAO"] = _CNPJFMT;
                    pCidade.Current["PERC_ISSQN"] = 0;
                    pCidade.Current["CODIGO_MUNICIPIO"] = pEndereco.CodigoIBGE;
                    pCidade.Current["ESTADO"] = pEndereco.Sigla.SafeUpper();
                    pCidade.Current["ENTRADA_ICMS_FRETE"] = "S";
                    pCidade.Current["NOME"] = pEndereco.Localidade.SafeUpper().RemoveAccent();
                    pCidade.Current["DATA_HORA_ALTERACAO"] = DateTime.Now;
                    pCidade.Current["CIDADE"] = cidpk;
                    pCidade.Flush();
                }
                else
                    cidpk = pCidade.Tuples.FirstOrDefault()["CIDADE"].AsString();
                return cidpk;
            }
        }
    }
}