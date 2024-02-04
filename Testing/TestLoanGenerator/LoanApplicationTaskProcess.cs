using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Interface.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationTaskProcess : LoanApplicationProcessObserver, ILoanApplicationTaskProcess
    {
        private readonly ProcessQueue<LoanApplication> _queue;
        private readonly ISettingsFactory _settingsFactory;
        private readonly Settings _settings;
        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private readonly ILogger _logger;
        private readonly List<ILoanApplicationProcessObserver> _observers = new List<ILoanApplicationProcessObserver>();
        private bool _disposedValue;

        public LoanApplicationTaskProcess(
            ISettingsFactory settingsFactory,
            Settings settings,
            IWorkTaskService workTaskService,
            IWorkTaskStatusService workTaskStatusService,
            ILogger logger)
        {
            _settingsFactory = settingsFactory;
            _settings = settings;
            _workTaskService = workTaskService;
            _workTaskStatusService = workTaskStatusService;
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
            List<WorkTask> workTasks = new List<WorkTask>();
            // the loan application work tasks are created async and there could be delay before they're created
            // here we're going to wait and retry until we find at least one.
            int attempts = 0;
            while (attempts < 30 && workTasks.Count == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                workTasks = await _workTaskService.GetByContext(
                    settings,
                    1,
                    loanApplication.LoanApplicationId.Value.ToString("D"));
            }
            List<Dictionary<string, object>> patchData = new List<Dictionary<string, object>>();
            foreach (WorkTask task in workTasks)
            {
                if (string.Equals(_settings.NewLoanApplicationTaskCode, task.WorkTaskType.Code, StringComparison.OrdinalIgnoreCase))
                {
                    NotifyNewLoanApplicationTaskCreate(loanApplication);
                }
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

        private void NotifyNewLoanApplicationTaskCreate(LoanApplication application)
        {
            foreach (ILoanApplicationProcessObserver observer in _observers)
            {
                observer.NewLoanApplicationTaskCreated(application);
            }
        }

        public void AddLoanApplication(LoanApplication application)
        {
            _queue.Enqueue(application);
        }

        public override Task LoanApplicationCreated(params LoanApplication[] loanApplications)
        {
            if (loanApplications != null)
            {
                for (int i = 0; i < loanApplications.Length; i += 1)
                {
                    AddLoanApplication(loanApplications[i]);
                }
            }
            return Task.CompletedTask;
        }

        public override Task LoanApplicationDenied(params LoanApplication[] loanApplications)
        {
            if (loanApplications != null)
            {
                for (int i = 0; i < loanApplications.Length; i += 1)
                {
                    AddLoanApplication(loanApplications[i]);
                }
            }
            return Task.CompletedTask;
        }

        public void AddObserver(ILoanApplicationProcessObserver observer)
            => _observers.Add(observer);

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

        public void WaitForProcessExit()
        {
            _queue.WaitForProcessExit();
        }
    }
}
