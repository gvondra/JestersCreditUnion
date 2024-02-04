using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanFactory
    {
        IAddressFactory AddressFactory { get; }
        ITransactionFacatory TransactionFacatory { get; }

        ILoan Create(ILoanApplication loanApplication);
        Task<ILoan> Get(ISettings settings, Guid id);
        Task<ILoan> GetByNumber(ISettings settings, string number);
        Task<IEnumerable<ILoan>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate);
        Task<IEnumerable<ILoan>> GetWithUnprocessedPayments(ISettings settings);
        Task<ILoan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId);
    }
}
