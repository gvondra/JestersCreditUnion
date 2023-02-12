using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class EmailAddressDataSaver : IEmailAddressDataSaver
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public EmailAddressDataSaver(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task Create(IDataSettings settings, EmailAddressData data)
        {
            data.CreateTimestamp = DateTime.UtcNow;
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<EmailAddressData>(Constants.CollectionName.EmailAddress)
                .InsertOneAsync(data)
                ;
        }

    }
}
