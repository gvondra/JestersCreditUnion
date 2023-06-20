using JestersCreditUnion.Framework;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class PhoneSaver : IPhoneSaver
    {
        public Task Create(ISettings settings, IPhone phone)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), phone.Create);
        }
    }
}
