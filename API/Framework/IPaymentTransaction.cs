using System.Threading.Tasks;
using JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Framework
{
    public interface IPaymentTransaction : ITransaction
    {
        public short TermNumber { get; set; }

        Task Create(ITransactionHandler transactionHandler);
    }
}
