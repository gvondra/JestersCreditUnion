using BrassLoon.DataClient;
using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IEmailAddressDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, EmailAddressData data);
    }
}
