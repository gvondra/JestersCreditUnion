using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IAmortizationService
    {
        Task<List<AmortizationItem>> Get(ISettings settings, Guid loanId);
    }
}
