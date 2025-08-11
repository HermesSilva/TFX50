using System;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Jobs;
using Projecao.Core.LGC.DB;

using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_PRODUTOS_CAMPOS_EMPRESAS), "2D4D92F3-8B0C-4848-8428-546C8573BE15", Tag = 1015)]
    public class FAT_PRODUTOS_CAMPOS_EMPRESAS : XBaseIntegracaoTabela<_ISExItemPrecoTipo, ISExItemPrecoTipo.XTuple>
    {
        private _ISExItem _Item;
        public override Int32 Order => 50;
        public override XLegacySide LegacySide => XLegacySide.Cloud;

        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_PRODUTOS_CAMPOS_EMPRESAS";

        protected override String RemoteTableWhere
        {
            get
            {
                if (XGerenciaIntegracao.Config.CNPJ.OnlyNumbers().SafeLength() != 14)
                    throw new XError("O CNPJ da configuração do Servidor de Integração está inválido");
                return $"EMPRESA_ID = '{XGerenciaIntegracao.Config.CNPJ.OnlyNumbers()}'";
            }
        }

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "SUSPENSO", "CODIGO_PRO", "EMPRESA_ID" };
            }
        }

        protected override void Prepare()
        {
            _Item = XPersistencePool.Get<_ISExItem>(Local);
            _Item.AddIndex("IDX01", ISExItem.ProdutoID);
            base.Prepare();
        }

        protected override void Release()
        {
            _Item.SafeDispose();
            base.Release();
        }

        protected override void FlushLocal()
        {
            _Item?.Flush();
            base.FlushLocal();
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            if (!Open())
                return;
            foreach (XDataTuple tgttpl in SourceResult)
            {
                String codpro = tgttpl["CODIGO_PRO"].As<String>();
                if (!_Item.LocateByIndex("IDX01", codpro) && !_Item.OpenAppend(ISExItem.ProdutoID, codpro))
                    continue;
                _Item.Current.SYSxEstadoID = tgttpl["SUSPENSO"].As<String>() == "N" ? SYSxEstado.XDefault.Ativo : SYSxEstado.XDefault.Inativo;
            }
        }
    }
}