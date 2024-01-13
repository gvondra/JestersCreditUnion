using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class NewLoanApplicationHandler : IMesssageHandler
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanApplicationService _loanApplicationService;

        public NewLoanApplicationHandler(ISettingsFactory settingsFactory, ILoanApplicationService loanApplicationService)
        {
            _settingsFactory = settingsFactory;
            _loanApplicationService = loanApplicationService;
        }

        public async Task Process(string messageBody)
        {
            Guid? loanApplicationId = GetLoanApplicationId(messageBody);
            if (loanApplicationId.HasValue)
            {
                LoanApplication loanApplication = await _loanApplicationService.Get(_settingsFactory.CreateLoan(), loanApplicationId.Value);
            }
        }

        private static Guid? GetLoanApplicationId(string messageBody)
        {
            Dictionary<string, string> body = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageBody);
            return body.ContainsKey("LoanApplicationId") && !string.IsNullOrEmpty(body["LoanApplicationId"]) ? Guid.Parse(body["LoanApplicationId"]) : null;
        }
    }
}
