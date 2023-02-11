using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    internal class MongoClientFactory : IMongoClientFactory
    {
        public async Task<MongoClient> CreateClient(IDataSettings dataSettings) 
        {
            MongoServerAddress mongoServerAddress = new MongoServerAddress(dataSettings.Host);
            MongoCredential credential = MongoCredential.CreatePlainCredential(dataSettings.DatabaseName, dataSettings.DatabaseUser, await dataSettings.GetDatabasePassword());
            MongoClientSettings clientSettings = new MongoClientSettings()
            {
                AllowInsecureTls = true,
                Credential = credential,
                Server = mongoServerAddress
            };
            MongoClient mongoClient = new MongoClient(clientSettings);
            return mongoClient;
        }

        public async Task<IMongoDatabase> GetDatabase(IDataSettings dataSettings)
        {
            MongoClient mongoClient = await CreateClient(dataSettings);
            return mongoClient.GetDatabase(dataSettings.DatabaseName);
        }
    }
}
