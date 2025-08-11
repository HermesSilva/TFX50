using System;
using System.Linq;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core.Model.DIC.ORM;

namespace Projecao.Core.ITG.Envio
{
    public class ItemPedidoTable : XBaseIntegracaoTabela
    {
        public override Int32 Order => 2;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "FAT_ITEMPEDIDO";

        public override string RemoteControlTable
        {
            get
            {
                return null;
            }
        }

        public override String GetCreateTrigger()
        {
            XORMLegacyTable capa = XGerenciaIntegracao.RemoteModel.Tables.FirstOrDefault(t => t.Name == RemoteTable);
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"CREATE OR REPLACE TRIGGER ITG_1019_TG");
            sb.AppendLineEx($"  AFTER INSERT ON {ORMRemoteTable.Name} FOR EACH ROW");
            sb.AppendLineEx($"DECLARE");
            sb.AppendLineEx($"  KEY VARCHAR2(80 BYTE);");
            sb.AppendLineEx($"BEGIN");
            sb.AppendLineEx($"  BEGIN");
            sb.AppendLineEx($"    SELECT ID_PEDIDO_PET INTO KEY FROM FAT_CAPAPEDIDO WHERE EMPRESA_ID = :NEW.EMPRESA_ID AND PEDIDO = :NEW.PEDIDO AND ID_PEDIDO_PET IS NOT NULL;");
            sb.AppendLineEx($"  EXCEPTION");
            sb.AppendLineEx($"    WHEN NO_DATA_FOUND THEN KEY := NULL;");
            sb.AppendLineEx($"  END;");
            sb.AppendLineEx($"  IF (KEY IS NOT NULL) THEN");
            sb.AppendLineEx($"    INSERT INTO ITG_DELECAO(TABELA, CHAVE) VALUES('{ORMRemoteTable.Name}', KEY);");
            sb.AppendLineEx($"  END IF;");
            sb.AppendLineEx($"END;");
            return sb.ToString();
        }

        protected override String[] TargetFieldSelect => throw new System.NotImplementedException();
    }
}