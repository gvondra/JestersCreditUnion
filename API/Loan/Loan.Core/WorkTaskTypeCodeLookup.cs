using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Constants;
using System.Threading.Tasks;
using ConfigAPI = BrassLoon.Interface.Config;

namespace JestersCreditUnion.Loan.Core
{
    public class WorkTaskTypeCodeLookup
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly ConfigAPI.IItemService _itemService;

        public WorkTaskTypeCodeLookup(SettingsFactory settingsFactory, ConfigAPI.IItemService itemService)
        {
            _settingsFactory = settingsFactory;
            _itemService = itemService;
        }

        public Task<string> GetNewLoanApplicationWorkTaskTypeCode(ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.NewLoanApplicationTaskTypeCode);

        public Task<string> GetSendDeinalCorrespondenceWorkTaskTypeCode(ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.SendDenialCorrespondenceTaskTypeCode);

        public Task<string> GetDiburseFundsTaskTypeCode(ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.DiburseFundsTaskTypeCode);

        private async Task<string> GetWorkTaskTypeCode(ISettings settings, string fieldName)
        {
            string result = string.Empty;
            ConfigurationSettings configurationSettings = _settingsFactory.CreateConfiguration(settings);
            dynamic configData = await _itemService.GetDataByCode(configurationSettings, settings.ConfigDomainId.Value, settings.WorkTaskConfigurationCode);
            if (configData != null)
                result = configData[fieldName] ?? string.Empty;
            return result;
        }
    }
}
