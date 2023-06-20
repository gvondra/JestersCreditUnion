using JestersCreditUnion.Framework;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class AddressSaver : IAddressSaver
    {
        public Task Create(ISettings settings, IAddress address)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), address.Create);
        }
    }
}
