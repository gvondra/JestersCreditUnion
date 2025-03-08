using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ILoanDataSaver
    {
        ILoanAgreementDataSaver LoanAgrementDataSaver { get; }
        Task Create(ITransactionHandler transactionHandler, LoanData data);
        Task Update(ITransactionHandler transactionHandler, LoanData data);
    }
}
