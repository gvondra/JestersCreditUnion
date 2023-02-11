using JestersCreditUnion.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class EmailAddressDataFactory : IEmailAddressDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public EmailAddressDataFactory(IMongoClientFactory mongoClientFactory) 
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<EmailAddressData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<EmailAddressData> filter = Builders<EmailAddressData>.Filter
                .Eq(ea => ea.EmailAddressId, id)
                ;
            return await (await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<EmailAddressData>(Constants.CollectionName.EmailAddress)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }

        public async Task<EmailAddressData> Get(IDataSettings settings, string address)
        {
            BsonRegularExpression bsonRegularExpression = new BsonRegularExpression(new Regex(@"^address$", RegexOptions.IgnoreCase));
            FilterDefinition<EmailAddressData> filter = Builders<EmailAddressData>.Filter
                .Regex(ea => ea.Address, bsonRegularExpression) 
                ;
            return await (await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<EmailAddressData>(Constants.CollectionName.EmailAddress)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }
    }
}
