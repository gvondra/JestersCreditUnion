using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data
{
    public interface ILoanBalanceDataFactory
    {
        Task<IEnumerable<LoanBalanceData>> GetAll(ISqlSettings settings);
    }
}
