using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class AddressSaver : IAddressSaver
    {
        public Task Create(ISettings settings, IAddress address)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), address.Create);
    }
}
