using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Interface.Models;
using JestersCreditUnion.Loan.Framework.Constants;
using JestersCreditUnion.Loan.Framework.Enumerations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class NewLoanApplicationHandler : IMesssageHandler
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILogger<NewLoanApplicationHandler> _logger;
        private readonly ILoanApplicationService _loanApplicationService;
        private readonly ILoanApplicationRatingService _loanApplicationRatingService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskTypeService _workTaskTypeService;
        private readonly IWorkTaskStatusService _workTaskStatusService;

        public NewLoanApplicationHandler(
            ISettingsFactory settingsFactory,
            ILogger<NewLoanApplicationHandler> logger,
            ILoanApplicationService loanApplicationService,
            ILoanApplicationRatingService loanApplicationRatingService,
            IWorkTaskService workTaskService,
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _logger = logger;
            _loanApplicationService = loanApplicationService;
            _loanApplicationRatingService = loanApplicationRatingService;
            _workTaskService = workTaskService;
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
        }

        public async Task Process(string messageBody)
        {
            Guid? loanApplicationId = GetLoanApplicationId(messageBody);
            if (loanApplicationId.HasValue)
            {
                LoanApplication loanApplication = await _loanApplicationService.Get(_settingsFactory.CreateLoan(), loanApplicationId.Value);
                await SetupWorkTask(loanApplication);
                await RateApplication(loanApplication);
            }
        }

        private async Task RateApplication(LoanApplication loanApplication)
        {
            try
            {
                await _loanApplicationRatingService.Create(_settingsFactory.CreateLoan(), loanApplication.LoanApplicationId.Value);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unable to rate loan application");
            }
        }

        private async Task SetupWorkTask(LoanApplication loanApplication)
        {
            string taskTypeCode = await GetNewLoanApplicationTaskTypeCode();
            if (!string.IsNullOrEmpty(taskTypeCode))
            {
                List<WorkTask> workTasks = await _workTaskService.GetByContext(
                _settingsFactory.CreateApi(),
                (short)WorkTaskContextReferenceType.LoanApplicationId,
                loanApplication.LoanApplicationId.Value.ToString("D"), false);
                int count = workTasks.Count(wt => string.Equals(wt.WorkTaskType?.Code ?? string.Empty, taskTypeCode, StringComparison.OrdinalIgnoreCase));
                if (count == 0)
                {
                    await CreateNewLoanApplicationWorkTask(taskTypeCode, loanApplication.LoanApplicationId.Value);
                }
            }
        }

        private async Task CreateNewLoanApplicationWorkTask(string taskTypeCode, Guid loanApplicationId)
        {
            WorkTaskStatus workTaskStatus = null;
            ApiSettings apiSettings = _settingsFactory.CreateApi();
            WorkTaskType workTaskType = await _workTaskTypeService.GetByCode(apiSettings, taskTypeCode);
            if (workTaskType?.WorkTaskTypeId.HasValue ?? false)
            {
                workTaskStatus = (await _workTaskStatusService.GetAll(apiSettings, workTaskType.WorkTaskTypeId.Value))
                    .FirstOrDefault(s => s.IsDefaultStatus ?? false);
            }
            if (workTaskType != null && workTaskStatus != null)
            {
                AsyncRetryPolicy retry = Policy.Handle<System.Exception>()
                        .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.667) });
                await retry.ExecuteAsync(() => CreateNewLoanApplicationWorkTask(apiSettings, workTaskType, workTaskStatus, loanApplicationId));
            }
        }

        private async Task CreateNewLoanApplicationWorkTask(
            ApiSettings settings,
            WorkTaskType workTaskType,
            WorkTaskStatus workTaskStatus,
            Guid loanApplicationId)
        {
            WorkTask workTask = new WorkTask
            {
                Text = "Review new loan application",
                Title = "Review new loan application",
                WorkTaskType = workTaskType,
                WorkTaskStatus = workTaskStatus,
                WorkTaskContexts = new List<WorkTaskContext>
                {
                    new WorkTaskContext
                    {
                        ReferenceType = (short)WorkTaskContextReferenceType.LoanApplicationId,
                        ReferenceValue = loanApplicationId.ToString("D")
                    }
                }
            };
            await _workTaskService.Create(settings, workTask);
        }

        private Task<string> GetNewLoanApplicationTaskTypeCode()
            => _workTaskTypeService.LookupCode(_settingsFactory.CreateApi(), WorkTaskConfigurationFields.NewLoanApplicationTaskTypeCode);

        private static Guid? GetLoanApplicationId(string messageBody)
        {
            Dictionary<string, string> body = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageBody);
            return body.ContainsKey("LoanApplicationId") && !string.IsNullOrEmpty(body["LoanApplicationId"]) ? Guid.Parse(body["LoanApplicationId"]) : null;
        }
    }
}
