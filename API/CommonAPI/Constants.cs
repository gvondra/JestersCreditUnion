﻿namespace JestersCreditUnion.CommonAPI
{
    public static class Constants
    {
        public const string AUTH_SCHEME_GOOGLE = "GoogleAuthentication";
        public const string AUTH_SCHEMA_JCU = "JestersCreditUnionAuthentication";
        public const string POLICY_BL_AUTH = "BL:AUTH"; // ensures the requestor used a brass loon token
        public const string POLICY_TOKEN_CREATE = "TOKEN:CREATE";
        public const string POLICY_WORKTASK_TYPE_READ = "WORKTASKTYPE:READ";
        public const string POLICY_WORKTASK_TYPE_EDIT = "WORKTASKTYPE:EDIT";
    }
}
