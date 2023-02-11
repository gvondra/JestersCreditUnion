using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
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
            await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.Phone)
                .InsertOneAsync(data);
            ;
        }

        public async Task Update(IDataSettings settings, LoanApplicationData data)
        {
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(la => la.LoanApplicationId, data.LoanApplicationId)
                ;
            await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.Phone)
                .ReplaceOneAsync(filter, data);
            ;
        }
    }
}
