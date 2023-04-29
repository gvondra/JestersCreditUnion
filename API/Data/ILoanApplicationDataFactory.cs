using JestersCreditUnion.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanApplicationDataFactory
    {
        Task<LoanApplicationData> Get(IDataSettings settings, Guid id);
        Task<IEnumerable<LoanApplicationData>> GetByUserId(IDataSettings settings, Guid userId);
        Task<IEnumerable<LoanApplicationData>> GetAll(IDataSettings settings);
    }
}
