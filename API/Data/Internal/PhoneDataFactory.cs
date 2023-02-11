using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PhoneDataFactory : IPhoneDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public PhoneDataFactory(IMongoClientFactory mongoClientFactory) 
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<PhoneData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<PhoneData> filter = Builders<PhoneData>.Filter
                .Eq(p => p.PhoneId, id)
                ;
            return await (await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<PhoneData>(Constants.CollectionName.Phone)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }

        public async Task<PhoneData> Get(IDataSettings settings, string number)
        {
            FilterDefinition<PhoneData> filter = Builders<PhoneData>.Filter
                .Eq(p => p.Number, number)
                ;
            return await (await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<PhoneData>(Constants.CollectionName.Phone)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }
    }
}
