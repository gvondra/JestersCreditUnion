using JestersCreditUnion.Interface.Models;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanTaskProcess : ILoanTaskProcess, ILoanProcessObserver
    {
        private readonly ILoanTaskProcess _process;
        private readonly ProcessQueue<Loan> _queue;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private bool _disposedValue;

        public LoanTaskProcess(
            ISettingsFactory settingsFactory,
            IWorkTaskService workTaskService,
            IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _workTaskService = workTaskService;
            _workTaskStatusService = workTaskStatusService;
            _process = this;
            _queue = new ProcessQueue<Loan>();
            _queue.ItemsDequeued += ItemsDequeued;
        }

        public void AddLoan(Loan loan)
        {
            _queue.Enqueue(loan);
        }

        public Task LoanCreated(ILoanProcess loanProcess, params Loan[] loans)
        {
            if (loans != null)
            {
                for (int i = 0; i < loans.Length; i += 1)
                {
                    _process.AddLoan(loans[i]);
                }
            }
            return Task.CompletedTask;
        }

        private void ItemsDequeued(object sender, System.Collections.Generic.IEnumerable<Loan> e)
        {
            if (e != null)
            {
                List<Task> tasks = new List<Task>();
                foreach (Loan loan in e)
                {
                    tasks.Add(
                        CloseWorkTasks(loan));
                }
                Task.WaitAll(tasks.ToArray());
            }
        }

        private async Task CloseWorkTasks(Loan loan)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            List<WorkTask> workTasks = await _workTaskService.GetByContext(
                settings,
                2,
                loan.LoanId.Value.ToString("D"));
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
                await _workTaskService.Patch(settings, patchData);
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
