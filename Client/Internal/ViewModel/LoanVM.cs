using JestersCreditUnion.Interface.Models;

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

        public static LoanVM Create(Loan loan)
        {
            LoanVM vm = new LoanVM(loan);
            return vm;
        }
    }
}
