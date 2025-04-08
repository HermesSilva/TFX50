using System;

using Microsoft.Extensions.DependencyInjection;

namespace TFX.Core.Interfaces
{
    public interface XIScoped
    {
    }

    public interface XIModule
    {
        void Initialize(IServiceCollection pServices);
    }

    public interface XIService 
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
