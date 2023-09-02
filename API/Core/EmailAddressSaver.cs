using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class EmailAddressSaver : IEmailAddressSaver
    {
        public Task Create(ISettings settings, IEmailAddress emailAddress)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), emailAddress.Create);
    }
}
