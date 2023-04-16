using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanApplicationDataFactory : ILoanApplicationDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public LoanApplicationDataFactory(IMongoClientFactory mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<LoanApplicationData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(a => a.LoanApplicationId, id)
                ;
            return await(await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.LoanApplication)
                .FindAsync(filter))
                .FirstOrDefaultAsync()
                ;
        }
    }
}
