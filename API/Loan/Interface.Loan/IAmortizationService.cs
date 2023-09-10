using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IAmortizationService
    {
        Task<List<AmortizationItem>> Get(ISettings settings, Guid loanId);
    }
}
