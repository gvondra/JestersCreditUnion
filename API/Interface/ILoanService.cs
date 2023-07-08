using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanService
    {
        Task<Loan> Get(ISettings settings, Guid id);
        Task<Loan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
        Task<Loan> GetByNumber(ISettings settings, string number);
        Task<List<Loan>> GetByBorrowerNameBirthDate(ISettings settings, string borrowerName, DateTime borrowerBirthDate);
        Task<Loan> Create(ISettings settings, Loan loan);
        Task<Loan> Update(ISettings settings, Loan loan);
        Task<Loan> Disbursement(ISettings settings, Guid id);
    }
}
