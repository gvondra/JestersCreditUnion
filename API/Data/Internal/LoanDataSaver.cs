using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanDataSaver : ILoanDataSaver
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public LoanDataSaver(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task Create(IDataSettings settings, LoanData data)
        {
            data.CreateTimestamp = DateTime.UtcNow;
            data.UpdateTimestamp = DateTime.UtcNow;
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanData>(Constants.CollectionName.Loan)
                .InsertOneAsync(data);
            ;
        }

        public async Task Update(IDataSettings settings, LoanData data)
        {
            FilterDefinition<LoanData> filter = Builders<LoanData>.Filter
                .Eq(ln => ln.LoanId, data.LoanId)
                ;
            data.UpdateTimestamp = DateTime.UtcNow;
            await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanData>(Constants.CollectionName.Loan)
                .ReplaceOneAsync(filter, data)
            ;
        }
    }
}
