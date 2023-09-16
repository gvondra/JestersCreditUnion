using System;

namespace JCU.Internal
{
    public class SettingsFactory : ISettingsFactory
    {
        public JestersCreditUnion.Interface.ISettings CreateApi()
        {
            if (string.IsNullOrEmpty(AccessToken.Get.Token))
                throw new ArgumentNullException(nameof(AccessToken.Token));
            return CreateApi(AccessToken.Get.Token);
        }

        public JestersCreditUnion.Interface.ISettings CreateApi(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            return new APISettings(Settings.ApiBaseAddress, token);
        }

        public JestersCreditUnion.Interface.Loan.ISettings CreateLoanApi()
        {
            if (string.IsNullOrEmpty(AccessToken.Get.Token))
                throw new ArgumentNullException(nameof(AccessToken.Token));
            return new LoanApiSettings(Settings.LoanApiBaseAddress, AccessToken.Get.Token);
        }
    }
}
