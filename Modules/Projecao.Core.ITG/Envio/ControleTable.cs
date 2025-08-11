using System;
using System.IO;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Recebe;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Service.Data;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Envio
{
    public class ControleTable : XBaseIntegracaoTabela<_SYSxPessoa, SYSxPessoa.XTuple>
    {
        public override int Order => 80;
        public override Guid ServerCID => RecebeControle.gCID;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "ITG_DELECAO";

        public override string FileName => Path.Combine(XDefault.TempFolder, "Controle.txt");

        public override string RemoteControlTable
        {
            get
            {
                return null;
            }
        }

        public override void Execute(XExecContext pContext)
        {
            base.Execute(pContext);
            Remote.DataBase.ExecSQL("DELETE FROM " + RemoteTable);
        }

        public override String GetCreateTrigger()
        {
            return null;
        }

        protected override string GetSelect()
        {
            return "SELECT * FROM ITG_DELECAO";
        }

        protected override string[] TargetFieldNames
        {
            get
            {
                return new[] { "TABELA", "CHAVE" };
            }
        }

        protected override string[] TargetFieldSelect
        {
            get
            {
                return new[] { "TABELA", "CHAVE" };
            }
        }

        protected override void AddJoins(StringBuilder pBuilder)
        {
        }

        protected override String RemoteTableWhere
        {
            get
            {
                return "";
            }
        }
    }
}