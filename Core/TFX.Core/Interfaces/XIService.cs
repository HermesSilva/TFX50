using System;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using TFX.Core.IDs.Model;

namespace TFX.Core.Interfaces
{
    public interface XIUseContext
    {
        void SetContextData(CancellationToken pCancellationToken, XUserSession pSession);      
    }

    public interface XIScoped
    {
    }

    public interface XIModule
    {
        void Initialize(IServiceCollection pServices);
    }

    public interface XIService : XIUseContext
    {
        Guid ID
        {
            get;
        }
        string Name
        {
            get;
        }
        bool LoadAll
        {
            get;
            set;
        }

        void GracefullyClose();
    }

    public interface XIJobService : XIService
    {
    }

    public interface XIJobRabbitMQService : XIJobService
    {
    }
}
