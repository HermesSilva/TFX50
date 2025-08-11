using Projecao.Core.ERP.GerenciaEmpresa;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.ERP.Test.GerenciaEmpresa
{
    [XRegister(typeof(GerenciaEmpresaSAMTootegaTestCITRule), "4A8AD133-259B-4BFC-80FC-648445BD8323", typeof(GerenciaEmpresaSAM), typeof(GerenciaEmpresaSAMTootegaTestCIT))]
    public class GerenciaEmpresaSAMTootegaTestCITRule : GerenciaEmpresaSAMTootegaTestCIT.XRule
    {
        public GerenciaEmpresaSAMTootegaTestCITRule()
        {
        }

        public override void Execute()
        {
            GerenciaEmpresaSVC.XTuple r = Model.DataSet[CurrentPlay];
            Search(GerenciaEmpresaSVC.GerenciaEmpresaFilterFRM.Fields.Nome, r.Sigla);
            DoEditSingle();

            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.Host, "VMDBServer")); // Servidor Oracle
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.Instancia, "orcl")); // Instância
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(ConfigITGFRM.Fields.Porta, 1521)); // Porta
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.Usuario, "LOJA")); // Usuário
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.Senha, "systbco")); // Senha
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.GrupoServico, "016.000")); // Grupo de Serviços
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.GrupoVacina, "013.000")); // Grupo de Vacinas
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.GrupoExame, "002.000")); // Grupo de Exames
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.HostInt, "cilauncher:12021")); // Servidor Integração
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.UsuarioInt, "ttg01@gestpet.com.br")); // Usuario Integração
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.SenhaInt, "senhas")); // Senha Integração
            //ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.TwilioNumero, r.TwilioNumero)); // Twilio Número
            //ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.DDD, r.DDD)); // DDD Prefrencial
            //ExecuteCommand(CurrentPlay, new XSendTextCommand(ConfigITGFRM.Fields.CodigoPais, r.CodigoPais)); // Código Pais (telefone)
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(ConfigITGFRM.Fields.LatenciaServico, 600)); // Latência do Serviço (s)
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(ConfigITGFRM.Fields.LatenciaIntegracao, 300)); // Latência da Integração (s)

            SaveRecord();
            Search(GerenciaEmpresaSVC.GerenciaEmpresaFilterFRM.Fields.Nome, r.Sigla);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}