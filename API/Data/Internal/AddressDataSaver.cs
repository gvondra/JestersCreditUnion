using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class AddressDataSaver : IAddressDataSaver
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public AddressDataSaver(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task Create(IDataSettings settings, AddressData data)
        {
            data.CreateTimestamp = DateTime.UtcNow;
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<AddressData>(Constants.CollectionName.Address)
                .InsertOneAsync(data)
                ;
        }

    }
}
