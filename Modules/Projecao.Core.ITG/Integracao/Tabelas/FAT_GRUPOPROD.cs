using System;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_GRUPOPROD), "BA9F2000-8822-45FD-A4DD-1DBDBC96E316", Tag = 1011)]
    public class FAT_GRUPOPROD : XBaseIntegracaoTabela<_ISExGrupo, ISExGrupo.XTuple>
    {
        public override Int32 Order => 10;

        public override XLegacySide LegacySide => XLegacySide.Cloud;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_GRUPOPROD";

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "NOME", "CODIGO_GRUPO" };
            }
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            if (!Open())
                return;
            foreach (XDataTuple tgttpl in SourceResult)
            {
                ISExGrupo.XTuple localtpl;
                String grpcod = tgttpl["CODIGO_GRUPO"].As<String>();
                if (LocalDBTable.OpenAppend(ISExGrupo.GrupoID, grpcod))
                    localtpl = LocalDBTable.Current;
                else
                    localtpl = LocalDBTable.NewTuple();
                String grupo = tgttpl["NOME"].As<String>();
                localtpl.Grupo = grupo;
                localtpl.GrupoID = grpcod;
            }
        }
    }
}