using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ITransactionFacatory
    {
        ITransaction Create(ILoan loan, DateTime date, TransactionType type, decimal amount);
        Task<IEnumerable<ITransaction>> GetByLoanId(ISettings settings, Guid loanId);
    }
}
