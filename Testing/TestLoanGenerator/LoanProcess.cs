using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanProcess : ILoanProcess, ILoanApplicationProcessObserver
    {
        private readonly ILoanProcess _process;
        private readonly ConcurrentQueue<LoanApplication> _loanApplications = new ConcurrentQueue<LoanApplication>();
        private readonly Thread _generateThread;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanService _loanService;
        private readonly ILoanPaymentAmountService _loanPaymentAmountService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskStatusService _workTaskStatusService;
        private bool _disposedValue;
        private bool _exit;

        public LoanProcess(
            ISettingsFactory settingsFactory,
            ILoanService loanService,
            ILoanPaymentAmountService loanPaymentAmountService,
            IWorkTaskService workTaskService,
            IWorkTaskStatusService workTaskStatusService)
        {
            _settingsFactory = settingsFactory;
            _loanService = loanService;
            _loanPaymentAmountService = loanPaymentAmountService;
            _workTaskService = workTaskService;
            _workTaskStatusService = workTaskStatusService;
            _process = this;
            _exit = false;
            _generateThread = new Thread(ProcessQueue)
            {
                IsBackground = true,
                Name = "Loan processor"
            };
            _generateThread.Start();
        }

        public void AddLoanApplication(LoanApplication application)
        {
            lock (_loanApplications)
            {
                _loanApplications.Enqueue(application);
                Monitor.PulseAll(_loanApplications);
            }
        }

        public void ProcessQueue()
        {
            IEnumerable<LoanApplication> loanApplications;
            while (!_exit || _loanApplications.Count > 0)
            {
                loanApplications = TryDeque();
                foreach (LoanApplication application in loanApplications)
                {
                    Task.WaitAll(
                        new Task[]
                        {
                            CloseWorkTasks(application),
                            CreateLoan(application)
                        });
                }
            }
        }

        private async Task CreateLoan(LoanApplication loanApplication)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            LoanAgreement agreement = new LoanAgreement
            {
                BorrowerAddress = loanApplication.BorrowerAddress,
                BorrowerBirthDate = loanApplication.BorrowerBirthDate,
                BorrowerEmailAddress = loanApplication.BorrowerEmailAddress,
                BorrowerName = loanApplication.BorrowerName,
                BorrowerPhone = loanApplication.BorrowerPhone,
                OriginalAmount = loanApplication.Amount,
                CreateDate = DateTime.Today,
                InterestRate = 0.02M,
                PaymentFrequency = 12,
                OriginalTerm = 48
            };
            Loan loan = new Loan
            {
                Agreement = agreement,
                LoanApplicationId = loanApplication.LoanApplicationId.Value                
            };
            loan.Agreement.PaymentAmount = await GetPaymentAmount(settings, loan);
            await _loanService.Create(settings, loan);
        }

        private async Task<decimal> GetPaymentAmount(ApiSettings settings, Loan loan)
        {
            LoanPaymentAmountRequest requet = new LoanPaymentAmountRequest
            {
                AnnualInterestRate = loan.Agreement.InterestRate,
                PaymentFrequency = loan.Agreement.PaymentFrequency.Value,
                Term = loan.Agreement.OriginalTerm.Value,
                TotalPrincipal = loan.Agreement.OriginalAmount.Value
            };
            LoanPaymentAmountResponse response = await _loanPaymentAmountService.Calculate(settings, requet);
            return response.PaymentAmount.Value;
        }

        private async Task CloseWorkTasks(LoanApplication loanApplication)
        {
            ApiSettings settings = await _settingsFactory.GetApiSettings();
            List<WorkTask> workTasks = await _workTaskService.GetByContext(
                settings,
                1,
                loanApplication.LoanApplicationId.Value.ToString("D"));
            foreach (WorkTask task in workTasks)
            {
                List<WorkTaskStatus> statuses = await _workTaskStatusService.GetAll(settings, task.WorkTaskType.WorkTaskTypeId.Value);
                task.WorkTaskStatus = statuses.First(s => s.IsClosedStatus ?? false);
                await _workTaskService.Update(settings, task);
            }
        }

        private IEnumerable<LoanApplication> TryDeque()
        {
            List<LoanApplication> result = new List<LoanApplication>();
            LoanApplication application;
            lock (_loanApplications)
            {
                while ((!_exit || _loanApplications.Count > 0) && result.Count == 0)
                {
                    application = null;
                    while (!_exit && !_loanApplications.TryDequeue(out application))
                    {
                        Monitor.Wait(_loanApplications);
                    }
                    if (application != null)
                        result.Add(application);
                    while (_loanApplications.TryDequeue(out application))
                    {
                        result.Add(application);
                    }
                }
                
            }
            return result;
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
                    Shutdown();
                    try
                    {
                        _generateThread.Join(60000);
                    }
                    catch (ThreadStateException) { }
                }
                _disposedValue = true;
            }
        }

        public void Shutdown()
        {
            lock (_loanApplications)
            {
                _exit = true;
                Monitor.PulseAll(_loanApplications);
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
