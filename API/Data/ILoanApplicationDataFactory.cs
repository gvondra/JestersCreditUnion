using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanApplicationDataFactory
    {
        Task<LoanApplicationData> Get(IDataSettings settings, Guid id);
    }
}
