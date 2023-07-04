using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace JCU.Internal.ViewModel
{
    public class BeginLoanAgreementVM : ViewModelBase
    {
        private readonly LoanVM _loan;
        private Visibility _busyVisibility = Visibility.Collapsed;
        private BeginLoanAgreementSave _save;
        private BeginLoanAgreementDisburse _disburse;
        private NavigationService _navigationService;

        private BeginLoanAgreementVM(LoanVM loanVM)
        {
            _loan = loanVM;
        }

        public LoanVM Loan => _loan;

        public NavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                if (_navigationService != value)
                {
                    _navigationService = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public BeginLoanAgreementSave Save
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

        public BeginLoanAgreementDisburse Disburse
        {
            get => _disburse;
            set
            {
                if (_disburse != value)
                {
                    _disburse = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility BusyVisibility
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static BeginLoanAgreementVM Create(LoanApplication loanApplication)
        {
            Loan loan = new Loan()
            {
                LoanApplicationId = loanApplication.LoanApplicationId,
                Agreement = new LoanAgreement()
                {
                    BorrowerBirthDate = loanApplication.BorrowerBirthDate,
                    BorrowerName = loanApplication.BorrowerName,
                    BorrowerEmailAddress = loanApplication.BorrowerEmailAddress,
                    BorrowerPhone = loanApplication.BorrowerPhone,
                    CoBorrowerBirthDate = loanApplication.CoBorrowerBirthDate,
                    CoBorrowerName = loanApplication.CoBorrowerName,
                    CoBorrowerEmailAddress = loanApplication.CoBorrowerEmailAddress,
                    CoBorrowerPhone = loanApplication.CoBorrowerPhone,
                    CreateDate = DateTime.Today,
                    OriginalAmount = loanApplication.Amount,
                    InterestRate = 0.0M,
                    PaymentFrequency = 12,
                    OriginalTerm = 48,
                    BorrowerAddress = new Address
                    {
                        Attention = loanApplication.BorrowerAddress.Attention,
                        City = loanApplication.BorrowerAddress.City,
                        Delivery = loanApplication.BorrowerAddress.Delivery,
                        PostalCode = loanApplication.BorrowerAddress.PostalCode,
                        Recipient = loanApplication.BorrowerAddress.Recipient,
                        Secondary = loanApplication.BorrowerAddress.Secondary,
                        State = loanApplication.BorrowerAddress.State
                    }
                }
            };
            if (loanApplication.CoBorrowerAddress != null)
            {
                loan.Agreement.CoBorrowerAddress = new Address()
                {
                    Attention = loanApplication.CoBorrowerAddress.Attention,
                    City = loanApplication.CoBorrowerAddress.City,
                    Delivery = loanApplication.CoBorrowerAddress.Delivery,
                    PostalCode = loanApplication.CoBorrowerAddress.PostalCode,
                    Recipient = loanApplication.CoBorrowerAddress.Recipient,
                    Secondary = loanApplication.CoBorrowerAddress.Secondary,
                    State = loanApplication.CoBorrowerAddress.State
                };
            }
            return Create(loan);
        }

        public static BeginLoanAgreementVM Create(Loan loan)
        {   
            BeginLoanAgreementVM vm = new BeginLoanAgreementVM(LoanVM.Create(loan));
            vm.Disburse = new BeginLoanAgreementDisburse();
            vm.Save = new BeginLoanAgreementSave();
            vm.AddBehavior(new BeginLoanAgreementValidator(vm));
            vm.AddBehavior(new BeginLoanAgreementLoader(vm));
            return vm;
        }
    }
}
