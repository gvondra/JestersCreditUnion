using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IIdentificationCardDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, IdentificationCardData data);
        Task Update(ISqlTransactionHandler transactionHandler, IdentificationCardData data);
    }
}
