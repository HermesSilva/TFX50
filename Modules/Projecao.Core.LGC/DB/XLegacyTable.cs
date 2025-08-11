using System;

using TFX.Core.Model.Data;
using TFX.Core.Service.Data;

namespace Projecao.Core.LGC.DB
{
    public enum XLegacySide : Int16
    {
        Shop = 0,
        Cloud = 1,
    }

    public enum XMigrationDirection
    {
        RemoteToLocal = 1,
        LocalToRemote = 2,
        Both = 3,
    }

    public enum XMigrationLevel
    {
        ServerToServer = 1,
        ClienteToServer = 2
    }

    public abstract class XLegacyTable
    {
        public XDataSet SourceResult;
        public Byte[] DataResult;

        public abstract Int32 Order
        {
            get;
        }

        public Guid LocalTableID
        {
            get;
            protected set;
        }

        public abstract String RemoteTable
        {
            get;
        }

        public abstract XLegacySide LegacySide
        {
            get;
        }

        public virtual String FileName
        {
            get;
        }

        public XExecContext Remote
        {
            get; set;
        }

        public XExecContext Local
        {
            get; set;
        }

        public virtual XMigrationLevel MigrationLevel => XMigrationLevel.ServerToServer;

        protected abstract XMigrationDirection MigrationDirection
        {
            get;
        }

        protected abstract String[] TargetFieldSelect
        {
            get;
        }

        protected virtual String[] TargetFieldNames => TargetFieldSelect;

        public abstract void Configure(XExecContext pRemote);

        public abstract void Prepare(XExecContext pSource, XExecContext pTarget);

        public abstract void Execute(XExecContext pContext);

        public virtual void DeleteTemp()
        {
        }
    }
}