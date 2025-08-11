using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Jobs;
using Projecao.Core.LGC.DB;
using Projecao.Core.PET.DB;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.Types;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_PRODUTOS), "D38F05AA-3250-45D1-9BDB-936B2E0F4D8E", Tag = 1014)]
    public class FAT_PRODUTOS : XBaseIntegracaoTabela<_ISExItem, ISExItem.XTuple>
    {
        private _ISExItemPreco _Preco;
        private _ISExLinha _Linha;
        private _ISExMarca _Marca;
        private _ISExGrupo _Grupo;
        private _ISExVacina _Vacina;
        private _ISExExame _Exame;
        private _ISExServico _Servico;
        private _ISExProduto _Produto;
        private _ISExComissao _Comissao;
        private _ISExItemClassificacao _Classifcacao;

        public override XLegacySide LegacySide => XLegacySide.Cloud;
        public override Int32 Order => 40;

        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_PRODUTOS";

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] {  "NOME_PRODUTO","CODIGO_PRO","PRECO_VENDA1","PRECO_MINIMO1","COMISSAO",
                                "PRECO_PROMOCAO","PRECO_PROMOCAO_VENCTO","MARCA_ID","CODIGO_LINHA","CODIGO_GRUPO" };
            }
        }

        protected override void Release()
        {
            base.Release();
            _Linha.SafeDispose();
            _Marca.SafeDispose();
            _Grupo.SafeDispose();
            _Preco.SafeDispose();
            _Vacina.SafeDispose();
            _Exame.SafeDispose();
            _Servico.SafeDispose();
            _Produto.SafeDispose();
            _Classifcacao.SafeDispose();
            _Comissao.SafeDispose();
        }

        protected override void Prepare()
        {
            base.Prepare();
            _Linha = XPersistencePool.Get<_ISExLinha>(Local);
            _Linha.AddIndex("IDX01", ISExLinha.LinhaID);

            _Marca = XPersistencePool.Get<_ISExMarca>(Local);
            _Marca.AddIndex("IDX01", ISExMarca.MarcaID);

            _Grupo = XPersistencePool.Get<_ISExGrupo>(Local);
            _Grupo.AddIndex("IDX01", ISExGrupo.GrupoID);
            _Comissao = XPersistencePool.Get<_ISExComissao>(Local);
            _Preco = XPersistencePool.Get<_ISExItemPreco>(Local);
            _Vacina = XPersistencePool.Get<_ISExVacina>(Local);
            _Exame = XPersistencePool.Get<_ISExExame>(Local);
            _Servico = XPersistencePool.Get<_ISExServico>(Local);
            _Produto = XPersistencePool.Get<_ISExProduto>(Local);
            _Classifcacao = XPersistencePool.Get<_ISExItemClassificacao>(Local);
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            Dictionary<String, Guid> icod = new Dictionary<String, Guid>();
            String where = "";
            if (XDefault.TestRunning)
            {
                String[] lines = null;

                using (StreamReader sr = new StreamReader(XResources.GetResourceStream(GetType().Assembly, "Itens.csv")))
                    lines = sr.ReadToEnd().SafeBreak(";");
                foreach (String ln in lines)
                {
                    String[] prts = ln.SafeBreak(",");
                    if (prts.SafeLength() == 2)
                        icod.Add(prts[1], new Guid(prts[0]));
                }
                XConsole.WriteLine($"Tota Itens Cod[{icod.Count}]");
                where = $" and (CODIGO_GRUPO in ('{XGerenciaIntegracao.Config.GrupoVacina}','{XGerenciaIntegracao.Config.GrupoServico}','{XGerenciaIntegracao.Config.GrupoExame}'))";
            }
            
            if (!Open(where))
                return;

            foreach (XDataTuple tgttpl in SourceResult)
            {
                ISExItem.XTuple localtpl;
                String produtoid = tgttpl["CODIGO_PRO"].As<String>();
                if (LocalDBTable.OpenAppend(ISExItem.ProdutoID, produtoid))
                    localtpl = LocalDBTable.Current;
                else
                    if (icod.ContainsKey(produtoid))
                    localtpl = LocalDBTable.NewTuple(icod[produtoid]);
                else
                    localtpl = LocalDBTable.NewTuple();
                String grupo = tgttpl["CODIGO_GRUPO"].AsString();
                localtpl.Nome = tgttpl["NOME_PRODUTO"].As<String>();
                localtpl.ProdutoID = produtoid;
                localtpl.ISExLinhaID = GetLinha(tgttpl["CODIGO_LINHA"].AsString());
                localtpl.ISExGrupoID = GetGrupo(grupo);
                localtpl.ISExMarcaID = GetMarca(XConvert.FromString<Int32>(tgttpl["MARCA_ID"].AsString()));
                AddPreco(tgttpl["PRECO_MINIMO1"].As<Decimal>(), localtpl.ISExItemID, ISExItemPrecoTipo.XDefault.Minimo, XDefault.NullDateTime, XDefault.Now.AddYears(100));
                AddPreco(tgttpl["PRECO_VENDA1"].As<Decimal>(), localtpl.ISExItemID, ISExItemPrecoTipo.XDefault.Normal_de_Venda, XDefault.NullDateTime, XDefault.Now.AddYears(100));
                Decimal promo = tgttpl["PRECO_PROMOCAO"].As<Decimal>();
                DateTime? datapromo = tgttpl["PRECO_PROMOCAO_VENCTO"].As<DateTime>();
                if (promo > 0 && datapromo.HasValue && datapromo > XDefault.Now)
                    AddPreco(promo, localtpl.ISExItemID, ISExItemPrecoTipo.XDefault.Promocional_Venda, XDefault.Now, datapromo.Value);

                if (Decimal.TryParse(tgttpl["COMISSAO"].AsString(), out Decimal comissao))
                    AddComissao(comissao, localtpl.ISExItemID);
                if (grupo == XGerenciaIntegracao.Config.GrupoExame)
                {
                    AddClassificacao(localtpl.ISExItemID, PETx.ISExItemClasse.XDefault.Exame);
                    if (!_Exame.OpenAppend(localtpl.ISExItemID))
                        _Exame.NewTuple(localtpl.ISExItemID);
                }
                else
                if (grupo == XGerenciaIntegracao.Config.GrupoServico)
                {
                    AddClassificacao(localtpl.ISExItemID, PETx.ISExItemClasse.XDefault.Servico);
                    if (!_Servico.OpenAppend(localtpl.ISExItemID))
                        _Servico.NewTuple(localtpl.ISExItemID);
                }
                else
                if (grupo == XGerenciaIntegracao.Config.GrupoVacina)
                {
                    AddClassificacao(localtpl.ISExItemID, PETx.ISExItemClasse.XDefault.Vacina);
                    AddClassificacao(localtpl.ISExItemID, ISExItemClasse.XDefault.Produto_Acabado);
                    if (!_Vacina.OpenAppend(localtpl.ISExItemID))
                        _Vacina.NewTuple(localtpl.ISExItemID);
                }
                else
                {
                    AddClassificacao(localtpl.ISExItemID, ISExItemClasse.XDefault.Produto_Acabado);
                    if (!_Produto.OpenAppend(localtpl.ISExItemID))
                        _Produto.NewTuple(localtpl.ISExItemID);
                }
                if (LocalDBTable.Count > 1000)
                    FlushLocal();
            }
            Local.Commit();
        }

        private void AddComissao(Decimal pComissao, Guid pSExItemID)
        {
            if (!_Comissao.OpenAppend(pSExItemID))
            {
                _Comissao.NewTuple();
                _Comissao.Current.ISExItemID = pSExItemID;
            }
            _Comissao.Current.Comissao = pComissao;
        }

        private Int32 GetMarca(Int32 pMarcaID)
        {
            if (_Marca.LocateByIndex("IDX01", pMarcaID))
                return _Marca.Current.ISExMarcaID;
            if (_Marca.OpenAppend(ISExMarca.MarcaID, pMarcaID))
                return _Marca.Current.ISExMarcaID;
            return 0;
        }

        private Int32 GetGrupo(String pGrupoID)
        {
            if (_Grupo.LocateByIndex("IDX01", pGrupoID))
                return _Grupo.Current.ISExGrupoID;
            if (_Grupo.OpenAppend(ISExGrupo.GrupoID, pGrupoID))
                return _Grupo.Current.ISExGrupoID;
            return 0;
        }

        private Int32 GetLinha(String pLinhaID)
        {
            if (_Linha.LocateByIndex("IDX01", pLinhaID))
                return _Linha.Current.ISExLinhaID;
            if (_Linha.OpenAppend(ISExLinha.LinhaID, pLinhaID))
                return _Linha.Current.ISExLinhaID;
            return 0;
        }

        private void AddClassificacao(Guid pISExItemID, Int16 pISExItemClasseID)
        {
            if (!_Classifcacao.OpenAppend(ISExItemClassificacao.ISExItemID, pISExItemID, ISExItemClassificacao.ISExItemClasseID, pISExItemClasseID))
            {
                _Classifcacao.NewTuple();
                _Classifcacao.Current.ISExItemClasseID = pISExItemClasseID;
                _Classifcacao.Current.ISExItemID = pISExItemID;
            }
        }

        protected override void FlushLocal()
        {
            if (XGerenciaIntegracao.IsClientReverse)
                return;
            base.FlushLocal();
            _Preco.Flush();
            _Vacina.Flush();
            _Exame.Flush();
            _Servico.Flush();
            _Produto.Flush();
            _Classifcacao.Flush();
            _Comissao.Flush();

            LocalDBTable.Clear();
            _Preco.Clear();
            _Vacina.Clear();
            _Exame.Clear();
            _Servico.Clear();
            _Produto.Clear();
            _Classifcacao.Clear();
            _Comissao.Clear();
        }

        private void AddPreco(Decimal pPreco, Guid pISExItemID, Int16 pISExItemPrecoTipoID, DateTime pInicio, DateTime pFim)
        {
            if (pPreco <= 0)
                return;
            ISExItemPreco.XTuple precotpl;
            if (pInicio == XDefault.NullDateTime)
            {
                if (_Preco.OpenAppend(ISExItemPreco.ISExItemID, pISExItemID, ISExItemPreco.ISExItemPrecoTipoID, pISExItemPrecoTipoID))
                    precotpl = _Preco.Current;
                else
                    precotpl = _Preco.NewTuple();
            }
            else
            {
                if (_Preco.OpenAppend(ISExItemPreco.ISExItemID, pISExItemID, ISExItemPreco.ISExItemPrecoTipoID, pISExItemPrecoTipoID, ISExItemPreco.FimValidade, pFim))
                    precotpl = _Preco.Current;
                else
                    precotpl = _Preco.NewTuple();
            }
            precotpl.ISExItemID = pISExItemID;
            precotpl.ISExItemPrecoTipoID = pISExItemPrecoTipoID;
            precotpl.Valor = pPreco;
            precotpl.InicioValidade = pInicio;
            precotpl.FimValidade = pFim;
        }
    }
}