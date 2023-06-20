using JestersCreditUnion.Framework;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class EmailAddressSaver : IEmailAddressSaver
    {
        public Task Create(ISettings settings, IEmailAddress emailAddress)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), emailAddress.Create);
        }
    }
}
