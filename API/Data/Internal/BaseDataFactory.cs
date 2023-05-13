using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public abstract class BaseDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        protected BaseDataFactory(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        protected async Task<IMongoCollection<T>> GetCollection<T>(IDataSettings settings, string collectionName)
        {
            return (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<T>(collectionName);
        }
    }
}
