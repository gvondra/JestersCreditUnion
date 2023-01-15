using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonAPI
{
    public static class Constants
    {
        public const string AUTH_SCHEME_GOOGLE = "GoogleAuthentication";
        public const string AUTH_SCHEMA_YARD_LIGHT = "YardLightAuthentication";
        public const string POLICY_BL_AUTH = "BL:AUTH"; // ensures the requestor used a brass loon token
        public const string POLICY_TOKEN_CREATE = "TOKEN:CREATE";
    }
}
