using System;
using System.IO;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.CEP.DB.CEPx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Integracao.Rule.Integracao.Tabelas
{
    [XRegister(typeof(FAT_CADASTROS), "45D9339A-95CC-4DEA-A910-2A1B6A2FF79C", Tag = 1010)]
    public class FAT_CADASTROS : XBaseIntegracaoTabela<_SYSxPessoa, SYSxPessoa.XTuple>
    {
        public override Int32 Order => 80;

        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override String RemoteTable => "FAT_CADASTROS";

        public override String FileName => Path.Combine(XDefault.TempFolder, "Pessoa.txt");

        protected override String[] TargetFieldNames
        {
            get
            {
                return new[] { "CODIGO_EXP", "CODIGO_MUNICIPIO", "ENDERECO", "BAIRRO", "COMPLEMENTO_ENDERECO", "CAD_CGC", "CEP",
                               "NOME", "TELEFONE", "E_MAIL" };
            }
        }

        protected override String[] TargetFieldSelect
        {
            get
            {
                return new[] { "CODIGO_EXP", "FAT_CIDADES.CODIGO_MUNICIPIO", "ENDERECO", "BAIRRO", "COMPLEMENTO_ENDERECO", "CAD_CGC", "CEP",
                               "COALESCE(NOME_FANTAZIA, RAZAO_SOCIAL) NOME", "TELEFONE", "E_MAIL" };
            }
        }

        protected override void AddJoins(StringBuilder pBuilder)
        {
            pBuilder.AppendLineEx("join FAT_CIDADES on FAT_CIDADES.CIDADE =  FAT_CADASTROS.CIDADE ");
        }

        protected override String RemoteTableWhere
        {
            get
            {
                return "FAT_CIDADES.CODIGO_MUNICIPIO IS NOT NULL ";
            }
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            if (!Open())
                return;
            if (!Directory.Exists(XDefault.TempFolder))
                Directory.CreateDirectory(XDefault.TempFolder);
            if (File.Exists(FileName))
                File.Delete(FileName);
            Int32 cnt = 0;
            Int32 cnt2 = 0;
            using (_CEPxLocalidade loc = XPersistencePool.Get<_CEPxLocalidade>(Local))
            using (XMemoryStream ms = new XMemoryStream())
            using (StreamWriter sw = new StreamWriter(ms, XEncoding.Default))
            {
                sw.AutoFlush = true;
                sw.WriteLine(String.Join(";", TargetFieldNames));
                foreach (XDataTuple tpl in SourceResult)
                {
                    String doc = tpl["CAD_CGC"].AsString();
                    if (!loc.ExistsInDB(CEPxLocalidade.CodigoIBGE, tpl["CODIGO_MUNICIPIO"].AsString()))
                    {
                        XConsole.Warn($"CodigoIBGE Inválido [{tpl["CODIGO_MUNICIPIO"]}].");
                        cnt2++;
                        continue;
                    }
                    if (!XUtils.CheckCNPJ(doc.OnlyNumbers()) && !XUtils.CheckCPF(doc.OnlyNumbers()))
                    {
                        XConsole.Warn($"DOC Inválido [{tpl["CAD_CGC"]}].");
                        cnt++;
                        continue;
                    }
                    foreach (String fld in TargetFieldNames)
                    {
                        String data = tpl[fld].AsString().SafeReplace(";", "");
                        if (fld == "CAD_CGC")
                            data = data.OnlyNumbers();
                        if (data.IsFull())
                            sw.Write(data);
                        sw.Write(";");
                    }
                    sw.WriteLine();
                }
                DataResult = ms.ToArray();
                XConsole.Warn($"DOC Inválidos [{cnt}/{cnt2}/{SourceResult.Count}].");
            }
        }
    }
}