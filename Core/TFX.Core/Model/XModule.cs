using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Interfaces;
using TFX.Core.Model.APP;

namespace TFX.Core.Model
{
    public abstract class XModule : XIModule
    {
        protected XModule()
        {
            Apps = new Dictionary<Guid, XAPPModel>();
        }
        public virtual void Initialize(IServiceCollection pServices)
        {
        }

        public Dictionary<Guid, XAPPModel> Apps
        {
            get; 
        }
    }
}
