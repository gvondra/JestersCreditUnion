using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPaymentFactory
    {
        ILoanFactory LoanFactory { get; }

        IPayment Create(
            ILoan loan,
            string transactionNumber,
            DateTime date);

        Task<IEnumerable<IPayment>> GetByLoanId(ISettings settings, Guid loanId);
    }
}
