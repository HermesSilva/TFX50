using System;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.ITG.Recebe;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Envio
{
    public abstract class XEnvios
    {
        public XEnvios()
        {
        }

        protected static XILog Log = XLog.GetLogFor(typeof(XEnvios));
        public XLegacyTable Table;
        public Int64 Size;

        public ConfigITGFRM.XCFGTuple Config;
        private int _RecordCount;

        public abstract Int32 RunnOrder

        {
            get;
        }
        public Int32 RecordCount => _RecordCount;

        public virtual Boolean CanExecute => true;

        public virtual void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            RecebeDadosSVC mdl = new RecebeDadosSVC();
            RecebeDadosSVC.XDataSet dst = mdl.PrepareDataSet<RecebeDadosSVC.XDataSet>();
            dst.NewTuple();
            dst.Current.Dados = Table.DataResult;
            _RecordCount = Table?.SourceResult?.Count ?? 0;
            Size = Table.DataResult.SafeLength();
            Byte[] data = pBroker.Put(dst);
            mdl.Load();
            using (XMemoryStream ms = new XMemoryStream(data))
                dst.SetDataEx(ms);
            if (!dst.Current.IsOk)
                throw new XError($"Falha no envio dos dados da tabela [{Table.RemoteTable}]");
        }
    }
}