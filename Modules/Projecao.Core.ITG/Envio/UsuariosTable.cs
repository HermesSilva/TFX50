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
    public class UsuariosTable : XBaseIntegracaoTabela<_SYSxPessoa, SYSxPessoa.XTuple>
    {
        public override int Order => 90;
        public override Guid ServerCID => RecebeUsuario.gCID;
        public override XLegacySide LegacySide => XLegacySide.Shop;
        protected override XMigrationDirection MigrationDirection => XMigrationDirection.RemoteToLocal;

        public override string RemoteTable => "FAT_FUNCIONARIOS";

        public override string FileName => Path.Combine(XDefault.TempFolder, "Usuario.txt");

        public override string RemoteControlTable => "ITG_1021";

        protected override bool UseDistinct => true;

        protected override Boolean IsValid(XDataTuple pTuple)
        {
            if (pTuple["FUNCIONARIO"].As<String>().IsEmpty())
                return false;
            if (pTuple["NOME"].As<String>().IsEmpty())
                return false;
            if (pTuple["SENHA"].As<String>().IsEmpty())
                return false;
            if (pTuple["NUMERO_EMPRESA"].As<String>().IsEmpty())
                return false;
            return true;
        }

        protected override string[] TargetFieldNames
        {
            get
            {
                return new[] { "FUNCIONARIO", "NOME", "SENHA", "NUMERO_EMPRESA" };
            }
        }

        protected override string[] TargetFieldSelect
        {
            get
            {
                return new[] { "FAT_FUNCIONARIOS.FUNCIONARIO", "FAT_FUNCIONARIOS.NOME", "FAT_FUNCIONARIOS.SENHA", "EMPRESAS.NUMERO_EMPRESA" };
            }
        }

        protected override void AddJoins(StringBuilder pBuilder)
        {
            pBuilder.AppendLineEx(" JOIN FAT_FUNCIONARIOEMPRESA ON FAT_FUNCIONARIOEMPRESA.FUNCIONARIO_ID = FAT_FUNCIONARIOS.FUNCIONARIO\r\n" +
                "  JOIN EMPRESAS ON EMPRESAS.EMPRESA_ID = FAT_FUNCIONARIOEMPRESA.EMPRESA_ID \r\n" +
                "  JOIN FAT_PARAMETROS ON FAT_PARAMETROS.EMPRESA_ID = FAT_FUNCIONARIOEMPRESA.EMPRESA_ID");
        }

        protected override string RemoteTableWhere
        {
            get
            {
                return "";
            }
        }
    }
}