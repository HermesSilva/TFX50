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
        }
        public virtual void Initialize(IServiceCollection pServices)
        {
        }
    }
}
