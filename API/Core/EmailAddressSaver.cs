using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class EmailAddressSaver : IEmailAddressSaver
    {
        private readonly Saver _saver;

        public EmailAddressSaver(Saver saver)
        {
            _saver = saver;
        }

        public Task Create(ISettings settings, params IEmailAddress[] emailAddresses)
        {
            if (emailAddresses == null) 
                throw new ArgumentNullException(nameof(emailAddresses));
            return _saver.Save(new TransactionHandler(settings),
                async th =>
                {
                    for (int i = 0; i < emailAddresses.Length; i += 1)
                    {
                        await emailAddresses[i].Create(th);
                    }
                });
        }
    }
}
