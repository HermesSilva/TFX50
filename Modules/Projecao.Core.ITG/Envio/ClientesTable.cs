using System;
using System.IO;
using System.Text;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Recebe;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Model.Data;
using TFX.Core.Utils;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ITG.Envio
{
    public class ClientesTable : XBaseIntegracaoTabela<_SYSxPessoa, SYSxPessoa.XTuple>
    {
        public override int Order => 80;
        public override Guid ServerCID => RecebeCliente.gCID;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "FAT_CADASTROS";

        public override string FileName => Path.Combine(XDefault.TempFolder, "Pessoa.txt");

        public override string RemoteControlTable
        {
            get
            {
                return "ITG_1010";
            }
        }

        protected override Boolean IsValid(XDataTuple pTuple)
        {
            if (pTuple["CEP"].As<String>().IsEmpty())
                return false;
            String doc = pTuple["CAD_CGC"].As<String>().OnlyNumbers();
            if (!doc.SafeLength().In(11, 14))
                return false;
            if (doc.SafeLength() == 14 && XUtils.CheckCNPJ(doc))
                return true;
            if (doc.SafeLength() == 11 && XUtils.CheckCPF(doc))
                return true;
            if (pTuple["CODIGO_MUNICIPIO"].As<String>().SafeLength() != 6)
                return false;
            return true;
        }

        protected override string[] TargetFieldNames
        {
            get
            {
                return new[] { "CODIGO_EXP", "CODIGO_MUNICIPIO", "ENDERECO", "BAIRRO", "COMPLEMENTO_ENDERECO", "CAD_CGC", "CEP",
                               "NOME", "TELEFONE", "E_MAIL" };
            }
        }

        protected override string[] TargetFieldSelect
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

        protected override string RemoteTableWhere
        {
            get
            {
                return "FAT_CIDADES.CODIGO_MUNICIPIO IS NOT NULL ";
            }
        }
    }
}