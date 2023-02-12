using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PhoneDataSaver : IPhoneDataSaver
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public PhoneDataSaver(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task Create(IDataSettings settings, PhoneData data)
        {
            data.CreateTimestamp = DateTime.UtcNow;
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<PhoneData>(Constants.CollectionName.Phone)
                .InsertOneAsync(data);
                ;
        }

    }
}
