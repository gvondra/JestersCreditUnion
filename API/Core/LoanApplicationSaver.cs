using BrassLoon.Interface.WorkTask.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Constants;
using JestersCreditUnion.Framework.Enumerations;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigAPI = BrassLoon.Interface.Config;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace JestersCreditUnion.Core
{
    public class LoanApplicationSaver : ILoanApplicationSaver
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly ConfigAPI.IItemService _itemService;
        private readonly WorkTaskAPI.IWorkTaskService _workTaskService;
        private readonly WorkTaskAPI.IWorkTaskTypeService _workTaskTypeService;
        private readonly WorkTaskAPI.IWorkTaskStatusService _workTaskStatusService;

        public LoanApplicationSaver(SettingsFactory settingsFactory, 
            ConfigAPI.IItemService itemService,
            WorkTaskAPI.IWorkTaskService workTaskService,
            WorkTaskAPI.IWorkTaskTypeService workTaskTypeService,
            WorkTaskAPI.IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _itemService = itemService;
            _workTaskService = workTaskService;
            _workTaskTypeService = workTaskTypeService;
            _workTaskStatusService = workTaskStatusService;
        }

        public async Task Create(ISettings settings, ILoanApplication loanApplication)
        {
            WorkTaskSettings workTaskSettings = _settingsFactory.CreateWorkTask(settings);
            WorkTaskType workTaskType = null;
            WorkTaskStatus workTaskStatus = null;
            string taskTypeCode = await GetNewLoanApplicationWorkTaskTypeCode(settings);
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

            await loanApplication.Create(settings);
            
            if (workTaskType != null && workTaskStatus != null)
            {
                AsyncRetryPolicy retry = Policy.Handle<Exception>()
                    .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.66) });
                await retry.ExecuteAsync(
                    () => CreateNewLoanApplicationWorkTask(settings, workTaskType, workTaskStatus, loanApplication.LoanApplicationId)
                    );
            }
        }

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

        private async Task<string> GetNewLoanApplicationWorkTaskTypeCode(ISettings settings)
        {
            string result = string.Empty;
            ConfigurationSettings configurationSettings = _settingsFactory.CreateConfiguration(settings);
            dynamic configData = await _itemService.GetDataByCode(configurationSettings, settings.ConfigDomainId.Value, settings.WorkTaskConfigurationCode);
            if (configData != null)
                result = configData[WorkTaskConfigurationFields.NewLoanApplicationTaskTypeCode] ?? string.Empty;
            return result;
        }
    }
}
