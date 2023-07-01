using Microsoft.AspNetCore.Authorization;
using System;

namespace JestersCreditUnion.CommonAPI
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public AuthorizationRequirement(string policyName, string issuer)
        {
            this.PolicyName = policyName;
            this.Issuer = issuer;
            this.Roles = Array.Empty<string>();
        }

        public AuthorizationRequirement(string policyName, string issuer, params string[] roles)
        {
            this.PolicyName = policyName;
            this.Issuer = issuer;
            this.Roles = roles;
        }

        public string PolicyName { get; set; }
        public string Issuer { get; set; }
        public string[] Roles { get; set; }
    }
}
