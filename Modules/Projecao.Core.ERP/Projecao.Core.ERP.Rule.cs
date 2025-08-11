using System;

using Projecao.Core.ERP.DB;

using TFX.Core.Model.DIC;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Cache;

namespace Projecao.Core.ERP
{
    [XRegister(typeof(AlmaCoreERPRule), sCID, typeof(ProjecaoCoreERP))]
    public class AlmaCoreERPRule : XModuleRule
    {
        public const String sCID = "3318E564-E0B4-4E43-9965-6DACC6A33B95";
        public static Guid gCID = new Guid(sCID);

        public AlmaCoreERPRule()
        {
            ID = gCID;
        }

        protected override void OnAfterServerLoaded()
        {
            XConfigCache.Reaload();
        }

        protected override void OnLoad()
        {
            XCustomerKey.OnLoadKey += KeyLoad;
        }

        private void KeyLoad(XCustomerKey pKey)
        {
            ERPx.ERPxPessoaJuridica.XTuple pjtpl = ERPx.ERPxPessoaJuridica.Instance.StaticData.NewTuple<ERPx.ERPxPessoaJuridica.XTuple>(pKey.ID);
            pjtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
            pjtpl.RazaoSocial = pKey.RazaoSocial;

            ERPx.ERPxDocumento.XTuple dctpl = ERPx.ERPxDocumento.Instance.StaticData.NewTuple<ERPx.ERPxDocumento.XTuple>(pKey.ERPxDocumentoID);
            dctpl.SYSxPessoaID = pKey.ID;
            dctpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
            dctpl.ERPxDocumentoTipoID = ERPx.ERPxDocumentoTipo.XDefault.CNPJ;
            dctpl.Numero = pKey.CNPJ;
        }
    }
}