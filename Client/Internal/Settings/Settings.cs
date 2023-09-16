using System.Configuration;

namespace JCU.Internal
{
    public static class Settings
    {
        public static string ApiBaseAddress => ConfigurationManager.AppSettings["ApiBaseAddress"];
        public static string LoanApiBaseAddress => ConfigurationManager.AppSettings["LoanApiBaseAddress"];
        public static string GoogleAuthorizationEndpoint => ConfigurationManager.AppSettings["GoogleAuthorizationEndpoint"];
        public static string GoogleClientId => ConfigurationManager.AppSettings["GoogleClientId"];
        public static string GoogleClientSecret => ConfigurationManager.AppSettings["GoogleClientSecret"];
        public static string GoogleTokenEndpoint => ConfigurationManager.AppSettings["GoogleTokenEndpoint"];
    }
}
