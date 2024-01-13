using JestersCreditUnion.Interface;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Constants;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class WorkTaskTypeCodeLookup : IWorkTaskTypeCodeLookup
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly IWorkTaskTypeService _workTaskTypeService;

        public WorkTaskTypeCodeLookup(SettingsFactory settingsFactory, IWorkTaskTypeService workTaskTypeService)
        {
            _settingsFactory = settingsFactory;
            _workTaskTypeService = workTaskTypeService;
        }

        public Task<string> GetNewLoanApplicationWorkTaskTypeCode(Framework.ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.NewLoanApplicationTaskTypeCode);

        public Task<string> GetSendDeinalCorrespondenceWorkTaskTypeCode(Framework.ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.SendDenialCorrespondenceTaskTypeCode);

        public Task<string> GetDiburseFundsTaskTypeCode(Framework.ISettings settings)
            => GetWorkTaskTypeCode(settings, WorkTaskConfigurationFields.DiburseFundsTaskTypeCode);

        public Task<string> GetWorkTaskTypeCode(Framework.ISettings settings, string fieldName)
            => _workTaskTypeService.LookupCode(_settingsFactory.CreateApi(settings), fieldName);
    }
}
