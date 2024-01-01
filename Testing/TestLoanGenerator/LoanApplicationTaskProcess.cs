using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Interface.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationTaskProcess : ILoanApplicationTaskProcess, ILoanApplicationProcessObserver
    {
        private readonly ILoanApplicationTaskProcess _process;
        private readonly ProcessQueue<LoanApplication> _queue;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private readonly ILogger _logger;
        private bool _disposedValue;

        public LoanApplicationTaskProcess(
            ISettingsFactory settingsFactory,
            IWorkTaskService workTaskService,
            IWorkTaskStatusService workTaskStatusService,
            ILogger logger)
        {
            _settingsFactory = settingsFactory;
            _workTaskService = workTaskService;
            _workTaskStatusService = workTaskStatusService;
            _process = this;
            _queue = new ProcessQueue<LoanApplication>();
            _queue.ItemsDequeued += LoanApplicationDequeued;
            _logger = logger;

        }

        private void LoanApplicationDequeued(object sender, IEnumerable<LoanApplication> e)
        {
            if (e != null)
            {
                List<Task> tasks = new List<Task>();
                foreach (LoanApplication application in e)
                {
                    tasks.Add(
                        CloseWorkTasks(application));
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task CloseWorkTasks(LoanApplication loanApplication)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            List<WorkTask> workTasks = await _workTaskService.GetByContext(
                settings,
                1,
                loanApplication.LoanApplicationId.Value.ToString("D"));
            List<Dictionary<string, object>> patchData = new List<Dictionary<string, object>>();
            foreach (WorkTask task in workTasks)
            {
                List<WorkTaskStatus> statuses = await _workTaskStatusService.GetAll(settings, task.WorkTaskType.WorkTaskTypeId.Value);
                WorkTaskStatus status = statuses.First(s => s.IsClosedStatus ?? false);
                patchData.Add(new Dictionary<string, object>
                {
                    { "WorkTaskId", task.WorkTaskId.Value.ToString("D") },
                    { "WorkTaskStatusId", status.WorkTaskStatusId.Value.ToString("D") }
                });
            }
            if (patchData.Count > 0)
            {
                _logger.Information($"Closing {patchData.Count} application tasks");
                await _workTaskService.Patch(settings, patchData);
            }   
        }

        public void AddLoanApplication(LoanApplication application)
        {
            _queue.Enqueue(application);
        }

        public Task LoanApplicationCreated(ILoanApplicationProcess loanApplicationProcess, params LoanApplication[] loanApplications)
        {
            if (loanApplications != null)
            {
                for (int i = 0; i < loanApplications.Length; i += 1)
                {
                    _process.AddLoanApplication(loanApplications[i]);
                }
            }
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _queue.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
