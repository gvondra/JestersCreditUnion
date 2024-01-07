using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanApplicationSummary
    {
        Task<List<LoanApplicationSummaryItem>> Get(ISettings settings, DateTime? minAppliationDate = null);
    }
}
