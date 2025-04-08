using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using TFX.Core.Interfaces;

namespace TFX.Core.Identity
{
    public interface XITenantProvider : XIScoped
    {
        string GetTenantID(bool pByPassTenat);
    }

    public class XTenantProvider : XITenantProvider
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public XTenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContextAccessor = httpContextAccessor;
        }

        private string _LastValue;

        public string GetTenantID(bool pByPassTenat)
        {
            if (pByPassTenat|| XDefault.TenantFieldName.IsEmpty())
                return null;
            if (_LastValue.IsFull())
                return _LastValue;
            var user = _HttpContextAccessor.HttpContext?.User;
            if (user == null)
                return null;

            var tenantId = user.FindFirst(XDefault.TenantFieldName);
            if (tenantId == null|| tenantId.Value.IsEmpty())
                return null;

            _LastValue = tenantId.Value;
            return _LastValue;
        }
    }
}
