using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class AddressDataFactory : IAddressDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public AddressDataFactory(IMongoClientFactory mongoClientFactory) 
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<AddressData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<AddressData> filter = Builders<AddressData>.Filter
                .Eq(a => a.AddressId, id)
                ;
            return await (await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<AddressData>(Constants.CollectionName.Address)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }

        public async Task<IEnumerable<AddressData>> GetByHash(IDataSettings settings, byte[] hash)
        {
            FilterDefinition<AddressData> filter = Builders<AddressData>.Filter
                .Eq(a => a.Hash, hash)
                ;
            return (await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<AddressData>(Constants.CollectionName.Address)
                .FindAsync(filter))
                .ToList()
                ;
        }
    }
}
