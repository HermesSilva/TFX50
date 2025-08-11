using System;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;
namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_LINHA), "AA145B44-E9BF-4DA4-B622-F0C52873CFD0", Tag = 1012)]
    public class FAT_LINHA : XBaseIntegracaoTabela<_ISExLinha, ISExLinha.XTuple>
    {
        public override Int32 Order => 20;

        public override XLegacySide LegacySide => XLegacySide.Cloud;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_LINHA";

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "NOME", "CODIGO_LINHA" };
            }
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            if (!Open())
                return;
            foreach (XDataTuple tgttpl in SourceResult)
            {
                ISExLinha.XTuple localtpl;
                String linhaid = tgttpl["CODIGO_LINHA"].As<String>();
                if (LocalDBTable.OpenAppend(ISExLinha.LinhaID, linhaid))
                    localtpl = LocalDBTable.Current;
                else
                    localtpl = LocalDBTable.NewTuple();
                localtpl.Linha = tgttpl["NOME"].As<String>();
                localtpl.LinhaID = linhaid;
            }
        }
    }
}