using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanAgreementDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, LoanAgreementData data);
        Task Update(ITransactionHandler transactionHandler, LoanAgreementData data);
    }
}
