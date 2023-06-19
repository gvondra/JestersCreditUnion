using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanApplicationDataFactory
    {
        Task<LoanApplicationData> Get(ISqlSettings settings, Guid id);
        Task<IEnumerable<LoanApplicationData>> GetByUserId(ISqlSettings settings, Guid userId);
    }
}
