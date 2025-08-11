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
    [XRegister(typeof(FAT_PROMOCAOITEM), "64A5B81C-8398-488B-A5E1-851787A2545C", Tag = 1017)]
    public class FAT_PROMOCAOITEM : XBaseIntegracaoTabela<_ISExItemPromocaoDetalhe, ISExItemPromocaoDetalhe.XTuple>
    {
        public override Int32 Order => 70;
        public override Guid ServerCID => RecebeDescontoDetalhe.gCID;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;
        public override Boolean CheckDelete => true;
        public override String RemoteTable => "FAT_PROMOCAOITEM";

        public override String FileName => Path.Combine(XDefault.TempFolder, "ItemPromocaoDetalhe.txt");

        protected override String[] TargetFieldNames
        {
            get
            {
                return new[] { "PRECO_PROMOCAO", "CODIGO_PRO", "PROMOCAO_ID" };
            }
        }

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "CAST(COALESCE(PRECO_PROMOCAO,0) AS NUMBER(25,2)) PRECO_PROMOCAO", "CODIGO_PRO", "PROMOCAO_ID" };
            }
        }

        protected override void AddJoins(StringBuilder pBuilder)
        {
        }

        protected override String RemoteTableWhere
        {
            get
            {
                return $"";
            }
        }
    }
}