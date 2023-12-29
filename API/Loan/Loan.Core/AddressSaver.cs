using JestersCreditUnion.Loan.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class AddressSaver : IAddressSaver
    {
        public Task Create(ISettings settings, IAddress address)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), (th) => address.Create(th, settings));
    }
}
