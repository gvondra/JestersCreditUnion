using JestersCreditUnion.Interface.Models;
using System;

namespace JCU.Internal.ViewModel
{
    public class LoanVM : ViewModelBase
    {
        private readonly Loan _innerLoan;
        private readonly LoanAgreementVM _agreement;

        private LoanVM (Loan innerLoan)
        {
            _innerLoan = innerLoan;
            _agreement = LoanAgreementVM.Create(innerLoan.Agreement);
        }

        public LoanAgreementVM Agreement => _agreement;

        public Loan InnerLoan => _innerLoan;

        public Guid? LoanId
        {
            get => _innerLoan.LoanId;
            set
            {
                if (_innerLoan.LoanId.HasValue != value.HasValue 
                    || (_innerLoan.LoanId.HasValue && _innerLoan.LoanId.Value != value.Value))
                {
                    _innerLoan.LoanId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LoanVM Create(Loan loan)
        {
            LoanVM vm = new LoanVM(loan);
            return vm;
        }
    }
}
