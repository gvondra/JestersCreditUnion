using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JCU.Internal
{
    public class AccessToken
    {
        private static AccessToken _instance;
        private const string Issuer = "urn:brassloon-derived";
        private static string _token;
        private JwtSecurityToken _jwtSecurityToken;
        private Dictionary<string, string> _googleToken;

        static AccessToken()
        {
            _instance = new AccessToken();
        }

        private AccessToken() { }

        public event PropertyChangedEventHandler PropertyChanged;

        public static AccessToken Get => _instance;

        public Dictionary<string, string> GoogleToken 
        { 
            get => _googleToken;
            set
            {
                _googleToken = value;
                NotifyPropertyChanged();
            }
        }

        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                if (!string.IsNullOrEmpty(value))
                    _jwtSecurityToken = new JwtSecurityToken(value);
                NotifyPropertyChanged();
            }
        }

        public string GetGoogleIdToken() => GoogleToken?["id_token"];

        public bool UserHasTaskTypeReadAccess() => UserHasRoleAccess("WORKTASKTYPE:READ");

        public bool UserHasTaskTypeEditAccess() => UserHasRoleAccess("WORKTASKTYPE:EDIT");

        public bool UserHasLookupEditAccess() => UserHasRoleAccess("LOOKUP:EDIT");

        public bool UserHasUserAdminRoleAccess() => UserHasRoleAccess("ROLE:EDIT");

        public bool UserHasLogReadAccess() => UserHasRoleAccess("LOG:READ");

        public bool UserHasClaimWorkTaskAccess() => UserHasRoleAccess("WORKTASK:CLAIM");

        public bool UserHasReadLoanAccess() => UserHasRoleAccess("LOAN:READ") || UserHasEditLoanAccess();

        public bool UserHasEditLoanAccess() => UserHasRoleAccess("LOAN:EDIT");

        public bool UserHasRoleAccess(string role)
        {
            return _jwtSecurityToken != null
                && _jwtSecurityToken.Claims.Any(
                    clm => string.Equals(clm.Issuer, Issuer, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(clm.Type, "role", StringComparison.OrdinalIgnoreCase)
                    && string.Equals(clm.Value, role, StringComparison.OrdinalIgnoreCase)
                    );
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
