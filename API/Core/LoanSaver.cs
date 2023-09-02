using BrassLoon.Interface.WorkTask.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace JestersCreditUnion.Core
{
    public class LoanSaver : ILoanSaver
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly WorkTaskTypeCodeLookup _workTaskTypeCodeLookup;
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        private readonly WorkTaskAPI.IWorkTaskStatusService _workTaskStatusService;
        private readonly WorkTaskAPI.IWorkTaskService _workTaskService;

        public LoanSaver(
            SettingsFactory settingsFactory,
            WorkTaskTypeCodeLookup workTaskTypeCodeLookup,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService,
            WorkTaskAPI.IWorkTaskStatusService workTaskStatusService,
            WorkTaskAPI.IWorkTaskService workTaskService)
        {
            _settingsFactory = settingsFactory;
            _workTaskTypeCodeLookup = workTaskTypeCodeLookup;
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
            _workTaskService = workTaskService;
        }

        public Task Create(ISettings settings, ILoan loan)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Create);

        public async Task InitiateDisburseFundsUpdate(ISettings settings, ILoan loan)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            WorkTaskType workTaskType = null;
            WorkTaskStatus workTaskStatus = null;
            string taskTypeCode = await _workTaskTypeCodeLookup.GetDiburseFundsTaskTypeCode(settings);
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
                await loan.Update(th);

                if (workTaskType != null && workTaskStatus != null)
                {
                    AsyncRetryPolicy retry = Policy.Handle<Exception>()
                        .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.667) });
                    await retry.ExecuteAsync(
                        () => CreateDisburseFundsWorkTask(settings, workTaskType, workTaskStatus, loan.LoanId)
                        );
                }
            });
        }

        public Task Update(ISettings settings, ILoan loan)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Update);


        private async Task CreateDisburseFundsWorkTask(
            ISettings settings,
            WorkTaskType workTaskType,
            WorkTaskStatus workTaskStatus,
            Guid loanId)
        {
            if (!await OpenWorkTaskExists(settings, loanId))
            {
                WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
                WorkTask workTask = new WorkTask
                {
                    DomainId = settings.WorkTaskDomainId,
                    Text = "Disburse Loan Funds",
                    Title = "Disburse Loan Funds",
                    WorkTaskType = workTaskType,
                    WorkTaskStatus = workTaskStatus,
                    WorkTaskContexts = new List<WorkTaskContext>
                {
                    new WorkTaskContext
                    {
                        DomainId= settings.WorkTaskDomainId,
                        ReferenceType = (short)WorkTaskContextReferenceType.LoanId,
                        ReferenceValue = loanId.ToString("D")
                    }
                }
                };
                await _workTaskService.Create(workTaskSettings, workTask);
            }
        }

        private async Task<bool> OpenWorkTaskExists(ISettings settings, Guid loanId)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            List<WorkTask> workTasks = await _workTaskService.GetByContext(workTaskSettings, settings.WorkTaskDomainId.Value, (short)WorkTaskContextReferenceType.LoanId, loanId.ToString("D"));
            return workTasks.Any(wt => !(wt.WorkTaskStatus.IsClosedStatus ?? false));
        }

    }
}
