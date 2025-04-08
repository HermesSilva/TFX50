using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Interfaces;

namespace TFX.Core.Model
{
    public abstract class XModule : XIModule
    {
        public virtual void Initialize(IServiceCollection pServices)
        {
        }
    }
}
