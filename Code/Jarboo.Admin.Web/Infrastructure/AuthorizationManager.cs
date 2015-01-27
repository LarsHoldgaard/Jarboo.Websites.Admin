using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

using Jarboo.Admin.DAL.Entities;

using Microsoft.Ajax.Utilities;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var action = context.Action.FirstOrDefault();
            if (action == null)
            {
                return true;
            }

            var controller = context.Resource.FirstOrDefault();
            if (controller == null)
            {
                return true;
            }

            foreach (var userRole in Rights.Roles)
            {
                if (context.Principal.IsInRole(userRole.ToString()))
                {
                    if (Rights.Can(userRole, controller.Value, action.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}