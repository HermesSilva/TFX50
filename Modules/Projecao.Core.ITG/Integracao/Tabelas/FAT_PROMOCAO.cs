using System;
using System.IO;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Recebe;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Reflections;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_PROMOCAO), "7577C908-74CC-4717-9D7E-A73E2D1AD6DB", Tag = 1016)]
    public class FAT_PROMOCAO : XBaseIntegracaoTabela<_ISExItemPromocao, ISExItemPromocao.XTuple>
    {
        public override Int32 Order => 60;
        public override Guid ServerCID => RecebeDesconto.gCID;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_PROMOCAO";

        public override String FileName => Path.Combine(XDefault.TempFolder, "ItemPromocao.txt");

        protected override String[] TargetFieldNames
        {
            get
            {
                return new[] { "FINAL", "INICIO", "PROMOCAO_ID" };
            }
        }

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "COALESCE(TO_CHAR(FINAL,'DD/MM/YYYY'),'01/01/1755') FINAL", "COALESCE(TO_CHAR(INICIO,'DD/MM/YYYY'),'01/01/1755') INICIO", "PROMOCAO_ID" };
            }
        }

        protected override void AddJoins(StringBuilder pBuilder)
        {
        }

        protected override String RemoteTableWhere
        {
            get
            {
                return $"FINAL is not null and INICIO is not null";
            }
        }
    }
}