using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PhoneSaver : IPhoneSaver
    {
        public Task Create(ISettings settings, IPhone phone)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), phone.Create);
    }
}
