using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanService
    {
        Task<Models.Loan> Get(ISettings settings, Guid id);
        Task<Models.Loan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
        Task<Models.Loan> GetByNumber(ISettings settings, string number);
        Task<List<Models.Loan>> GetByBorrowerNameBirthDate(ISettings settings, string borrowerName, DateTime borrowerBirthDate);
        Task<Models.Loan> Create(ISettings settings, Models.Loan loan);
        Task<Models.Loan> Update(ISettings settings, Models.Loan loan);
        Task<Models.Loan> InitiateDisbursement(ISettings settings, Guid id);
        Task<Models.DisburseResponse> DisburseFunds(ISettings settings, Guid id);
    }
}
