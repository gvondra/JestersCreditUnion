using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPhoneDataSaver
    {
        Task Create(ISqlTransactionHandler transactionHandler, PhoneData data);
    }
}
