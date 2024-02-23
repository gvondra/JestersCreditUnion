﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonAPI
{
    public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated &&
                IssuerMatches(context.User, requirement.Issuer) &&
                RoleMatches(context.User, requirement.Roles))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        private static bool RoleMatches(ClaimsPrincipal user, string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                return Array.Exists(roles, role => user.Claims.Any(
                    c => string.Equals(ClaimTypes.Role, c.Type, StringComparison.OrdinalIgnoreCase) && string.Equals(role, c.Value, StringComparison.OrdinalIgnoreCase)));
            }
            return true;
        }

        private static bool IssuerMatches(ClaimsPrincipal user, string issuer)
        {
            if (!string.IsNullOrEmpty(issuer) && user.Claims.Any(c => string.Equals("iss", c.Type, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issuer, c.Value, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }
    }
}
