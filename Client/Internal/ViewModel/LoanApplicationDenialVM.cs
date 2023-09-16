using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Loan.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationDenialVM : ViewModelBase
    {
        private readonly LoanApplication _loanApplication;
        private LookupVM _reasonLookup;
        private LoanApplicationDenialSave _save;

        private LoanApplicationDenialVM(LoanApplication loanApplication)
        {
            if (loanApplication.Denial == null)
                loanApplication.Denial = new LoanApplicationDenial();
            _loanApplication = loanApplication;
        }

        public LoanApplicationDenial InnerLoanApplicationDenial => _loanApplication.Denial;

        public string StatusDescription => _loanApplication.StatusDescription;

        public Guid LoanApplicationId => _loanApplication.LoanApplicationId.Value;

        public Guid? UserId => _loanApplication.Denial.UserId;

        public string UserName
        {
            get => _loanApplication.Denial.UserName;
            set
            {
                if (_loanApplication.Denial.UserName != value)
                {
                    _loanApplication.Denial.UserName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LoanApplicationDenialSave Save
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LookupVM ReasonLookup
        {
            get => _reasonLookup;
            set
            {
                if (_reasonLookup != value)
                {
                    _reasonLookup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public short Reason
        {
            get => _loanApplication.Denial.Reason ?? 0;
            set
            {
                if (!_loanApplication.Denial.Reason.HasValue || _loanApplication.Denial.Reason != value)
                {
                    _loanApplication.Denial.Reason = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _loanApplication.Denial.Date ?? DateTime.Today;
            set
            {
                if (!_loanApplication.Denial.Date.HasValue || _loanApplication.Denial.Date != value)
                {
                    _loanApplication.Denial.Date = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Text
        {
            get => _loanApplication.Denial.Text ?? string.Empty;
            set
            {
                if (_loanApplication.Denial.Text != value)
                {
                    _loanApplication.Denial.Text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LoanApplicationDenialVM Create(LoanApplication loanApplication)
        {
            LoanApplicationDenialVM vm = new LoanApplicationDenialVM(loanApplication);
            vm.Save = new LoanApplicationDenialSave();
            LoanApplicationDenialValidator validator = new LoanApplicationDenialValidator(vm);
            vm.AddBehavior(validator);
            LoanApplicationDenialLoader loader = new LoanApplicationDenialLoader(vm);
            vm.AddBehavior(loader);
            loader.LoadReasonsLookup();
            loader.LoadUserName();
            return vm;
        }
    }
}
