using System;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;
namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_MARCAS), "2D46D422-3E6F-4182-87BE-765FB7A0FAFA", Tag = 1013)]
    public class FAT_MARCAS : XBaseIntegracaoTabela<_ISExMarca, ISExMarca.XTuple>
    {
        public override Int32 Order => 30;

        public override XLegacySide LegacySide => XLegacySide.Cloud;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_MARCAS";

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "NOME_MARCA", "MARCA_ID" };
            }
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            if (!Open())
                return;
            foreach (XDataTuple tgttpl in SourceResult)
            {
                ISExMarca.XTuple localtpl;
                Int32 marcaid = XConvert.FromString<Int32>(tgttpl["MARCA_ID"].AsString());
                if (LocalDBTable.OpenAppend(ISExMarca.MarcaID, marcaid))
                    localtpl = LocalDBTable.Current;
                else
                    localtpl = LocalDBTable.NewTuple();
                localtpl.Marca = tgttpl["NOME_MARCA"].As<String>();
                localtpl.MarcaID = marcaid;
            }
        }
    }
}