using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class LoanAgreementVM
    {
        private readonly LoanAgreement _innerLoanAgreement;

        private LoanAgreementVM(LoanAgreement innerLoanAgreement)
        {
            _innerLoanAgreement = innerLoanAgreement;
        }

        public static LoanAgreementVM Create(LoanAgreement loanAgreement)
        {
            LoanAgreementVM vm = new LoanAgreementVM(loanAgreement);
            return vm;
        }
    }
}
