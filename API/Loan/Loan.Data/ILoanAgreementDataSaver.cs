using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface ILoanAgreementDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, LoanAgreementData data);
        Task Update(ITransactionHandler transactionHandler, LoanAgreementData data);
    }
}
