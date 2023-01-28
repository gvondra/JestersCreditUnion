using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PhoneSaver : IPhoneSaver
    {
        private readonly Saver _saver;

        public PhoneSaver(Saver saver)
        {
            _saver = saver;
        }

        public Task Create(ISettings settings, params IPhone[] phones)
        {
            if (phones == null)
                throw new ArgumentNullException(nameof(phones));
            return _saver.Save(new TransactionHandler(settings),
                async th =>
                {
                    for (int i = 0; i < phones.Length; i += 1) 
                    {
                        await phones[i].Create(th);
                    }
                });
        }
    }
}
