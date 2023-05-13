using JestersCreditUnion.Data.Models;
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
    }
}
