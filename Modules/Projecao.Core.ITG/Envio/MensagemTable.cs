using System;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

namespace Projecao.Core.ITG.Envio
{
    public class MensagemTable : XBaseIntegracaoTabela
    {
        public override Int32 Order => 1;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "FAT_SERVICOMENSAGEM";

        public override string RemoteControlTable
        {
            get
            {
                return null;
            }
        }

        public override String GetCreateTrigger()
        {
            return null;
        }

        protected override String[] TargetFieldSelect => throw new System.NotImplementedException();
    }
}