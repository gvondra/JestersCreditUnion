using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class LoanVM
    {
        private readonly Loan _innerLoan;
        private readonly LoanAgreementVM _agreement;

        private LoanVM (Loan innerLoan)
        {
            _innerLoan = innerLoan;
            _agreement = LoanAgreementVM.Create(innerLoan.Agreement);
        }

        public static LoanVM Create(Loan loan)
        {
            LoanVM vm = new LoanVM(loan);
            return vm;
        }
    }
}
