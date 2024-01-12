using BrassLoon.Interface.WorkTask.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationSaver : ILoanApplicationSaver
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly WorkTaskAPI.IWorkTaskService _workTaskService;
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        private readonly WorkTaskAPI.IWorkTaskStatusService _workTaskStatusService;
        private readonly WorkTaskTypeCodeLookup _workTaskTypeCodeLookup;

        public LoanApplicationSaver(SettingsFactory settingsFactory,
            WorkTaskAPI.IWorkTaskService workTaskService,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService,
            WorkTaskAPI.IWorkTaskStatusService workTaskStatusService,
            WorkTaskTypeCodeLookup workTaskTypeCodeLookup)
        {
            _settingsFactory = settingsFactory;
            _workTaskService = workTaskService;
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
            _workTaskTypeCodeLookup = workTaskTypeCodeLookup;
        }

        public async Task Create(ISettings settings, ILoanApplication loanApplication)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            WorkTaskType workTaskType = null;
            WorkTaskStatus workTaskStatus = null;
            string taskTypeCode = await _workTaskTypeCodeLookup.GetNewLoanApplicationWorkTaskTypeCode(settings);
            if (!string.IsNullOrEmpty(taskTypeCode))
            {
                workTaskType = await _workTaskTypeService.GetByCode(workTaskSettings, settings.WorkTaskDomainId.Value, taskTypeCode);
            }
            if (workTaskType?.WorkTaskTypeId.HasValue ?? false)
            {
                workTaskStatus = (await _workTaskStatusService.GetAll(workTaskSettings, settings.WorkTaskDomainId.Value, workTaskType.WorkTaskTypeId.Value))
                    .FirstOrDefault(s => s.IsDefaultStatus ?? false);
            }
            else
            {
                throw new ApplicationException($"Work task type not found with code {taskTypeCode} in domain {settings.WorkTaskDomainId.Value:D}");
            }

            await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), async th =>
            {
                await loanApplication.Create(th, settings);

                if (workTaskType != null && workTaskStatus != null)
                {
                    AsyncRetryPolicy retry = Policy.Handle<Exception>()
                        .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.667) });
                    await retry.ExecuteAsync(
                        () => CreateNewLoanApplicationWorkTask(settings, workTaskType, workTaskStatus, loanApplication.LoanApplicationId));
                }

                await ServiceBusService.NewLoanApplicationEnqueue(settings, loanApplication.LoanApplicationId);
            });
        }

        public Task CreateComment(ISettings settings, ILoanApplicationComment loanApplicationComment)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loanApplicationComment.Create);

        public async Task Deny(ISettings settings, ILoanApplication loanApplication)
        {
            ILoanApplicationDenial denial = loanApplication.Denial;
            if (denial != null)
            {
                WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
                WorkTaskType workTaskType = null;
                WorkTaskStatus workTaskStatus = null;
                string taskTypeCode = await _workTaskTypeCodeLookup.GetSendDeinalCorrespondenceWorkTaskTypeCode(settings);
                if (!string.IsNullOrEmpty(taskTypeCode))
                {
                    workTaskType = await _workTaskTypeService.GetByCode(workTaskSettings, settings.WorkTaskDomainId.Value, taskTypeCode);
                }
                if (workTaskType?.WorkTaskTypeId.HasValue ?? false)
                {
                    workTaskStatus = (await _workTaskStatusService.GetAll(workTaskSettings, settings.WorkTaskDomainId.Value, workTaskType.WorkTaskTypeId.Value))
                        .FirstOrDefault(s => s.IsDefaultStatus ?? false);
                }

                await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), async th =>
                {
                    await denial.Save(th, loanApplication.LoanApplicationId, loanApplication.Status, loanApplication.ClosedDate);

                    if (workTaskType != null && workTaskStatus != null)
                    {
                        AsyncRetryPolicy retry = Policy.Handle<Exception>()
                            .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.667) });
                        await retry.ExecuteAsync(
                            () => CreateSendDenialCorrespondenceWorkTask(settings, workTaskType, workTaskStatus, loanApplication.LoanApplicationId)
                            );
                    }
                });
            }
        }

        public Task Update(ISettings settings, ILoanApplication loanApplication)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), (th) => loanApplication.Update(th, settings));

        private async Task CreateNewLoanApplicationWorkTask(
            ISettings settings,
            WorkTaskType workTaskType,
            WorkTaskStatus workTaskStatus,
            Guid loanApplicationId)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            WorkTask workTask = new WorkTask
            {
                DomainId = settings.WorkTaskDomainId,
                Text = "Review new loan application",
                Title = "Review new loan application",
                WorkTaskType = workTaskType,
                WorkTaskStatus = workTaskStatus,
                WorkTaskContexts = new List<WorkTaskContext>
                {
                    new WorkTaskContext
                    {
                        DomainId= settings.WorkTaskDomainId,
                        ReferenceType = (short)WorkTaskContextReferenceType.LoanApplicationId,
                        ReferenceValue = loanApplicationId.ToString("D")
                    }
                }
            };
            await _workTaskService.Create(workTaskSettings, workTask);
        }

        private async Task CreateSendDenialCorrespondenceWorkTask(
            ISettings settings,
            WorkTaskType workTaskType,
            WorkTaskStatus workTaskStatus,
            Guid loanApplicationId)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            WorkTask workTask = new WorkTask
            {
                DomainId = settings.WorkTaskDomainId,
                Text = "Send denial correspondence",
                Title = "Send denial correspondence",
                WorkTaskType = workTaskType,
                WorkTaskStatus = workTaskStatus,
                WorkTaskContexts = new List<WorkTaskContext>
                {
                    new WorkTaskContext
                    {
                        DomainId= settings.WorkTaskDomainId,
                        ReferenceType = (short)WorkTaskContextReferenceType.LoanApplicationId,
                        ReferenceValue = loanApplicationId.ToString("D")
                    }
                }
            };
            await _workTaskService.Create(workTaskSettings, workTask);
        }
    }
}
