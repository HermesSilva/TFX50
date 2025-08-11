using System;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

namespace Projecao.Core.ITG.Envio
{
    public class PedidoTable : XBaseIntegracaoTabela
    {
        public override Int32 Order => 1;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "FAT_CAPAPEDIDO";

        public override string RemoteControlTable
        {
            get
            {
                return null;
            }
        }

        public override String GetCreateTrigger()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"CREATE OR REPLACE TRIGGER ITG_1018_TG");
            sb.AppendLineEx($"  AFTER UPDATE OR DELETE ON {ORMRemoteTable.Name} FOR EACH ROW");
            sb.AppendLineEx($"BEGIN");
            sb.AppendLineEx($"  IF (:OLD.STATUS = 'CAN' AND :OLD.ID_PEDIDO_PET IS NOT NULL) THEN");
            sb.AppendLineEx($"    INSERT INTO ITG_DELECAO(TABELA, CHAVE) VALUES('{ORMRemoteTable.Name}', :OLD.ID_PEDIDO_PET);");
            sb.AppendLineEx($"  END IF;");
            sb.AppendLineEx($"END;");
            return sb.ToString();
        }

        protected override String[] TargetFieldSelect => throw new System.NotImplementedException();
    }
}