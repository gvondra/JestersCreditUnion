using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanApplicationDataSaver : ILoanApplicationDataSaver
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public LoanApplicationDataSaver(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task Create(IDataSettings settings, LoanApplicationData data)
        {
            data.CreateTimestamp = DateTime.UtcNow;
            data.UpdateTimestamp = DateTime.UtcNow;
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.LoanApplication)
                .InsertOneAsync(data);
            ;
        }

        public async Task Update(IDataSettings settings, LoanApplicationData data)
        {
            data.UpdateTimestamp = DateTime.UtcNow;
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(la => la.LoanApplicationId, data.LoanApplicationId)
                ;
            await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.LoanApplication)
                .ReplaceOneAsync(filter, data);
            ;
        }
    }
}
