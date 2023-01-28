using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class AddressSaver : IAddressSaver
    {
        private readonly Saver _saver;

        public AddressSaver(Saver saver)
        {
            _saver = saver;
        }

        public Task Create(ISettings settings, params IAddress[] addresses)
        {
            if (addresses == null) 
                throw new ArgumentNullException(nameof(addresses));
            return _saver.Save(new TransactionHandler(settings),
                async th =>
                {
                    for (int i = 0; i < addresses.Length; i += 1)
                    {
                        await addresses[i].Create(th);
                    }
                });
        }
    }
}
