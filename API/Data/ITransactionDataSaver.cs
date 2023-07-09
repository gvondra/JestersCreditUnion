using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ITransactionDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, TransactionData data);
    }
}
