
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.AspNetCore.Http;

using TFX.Core.Exceptions;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;
using TFX.Core.Model;

namespace TFX.Core.Data
{
    public static class XContextExtension
    {
        public static void InternalSetContext(this XIUseContext pCancelable, CancellationToken pCancellationToken, XUserSession pSession)
        {
            var fields = pCancelable.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields.Where(f => typeof(XIUseContext).IsAssignableFrom(f.FieldType)))
                if (field.GetValue(pCancelable) is XIUseContext cancelable)
                    cancelable.SetContextData(pCancellationToken, pSession);

        }
    }

    public abstract class XUseContext : XIUseContext
    {
        private CancellationToken _CancellationToken;
        private XUserSession _Session;

        protected XUserSession Session => _Session;
        protected CancellationToken CancellationToken => _CancellationToken;

        public virtual void SetContextData(CancellationToken pCancellationToken, XUserSession pSession)
        {
            _CancellationToken = pCancellationToken;
            _Session = pSession;
            this.InternalSetContext(pCancellationToken, pSession);
            CancellationToken.Register(OnCancel);
        }

        protected virtual void OnCancel()
        {
        }
    }
}
