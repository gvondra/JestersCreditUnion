using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanApplicationDataFactory : BaseDataFactory, ILoanApplicationDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public LoanApplicationDataFactory(IMongoClientFactory mongoClientFactory)
            : base(mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<LoanApplicationData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(a => a.LoanApplicationId, id)
                ;
            return (await (await GetCollection<LoanApplicationData>(settings, Constants.CollectionName.LoanApplication))
                .FindAsync(filter))
                .FirstOrDefault()
                ;
        }

        public async Task<IEnumerable<LoanApplicationData>> GetAll(IDataSettings settings)
        {
            SortDefinition<LoanApplicationData> sort = Builders<LoanApplicationData>.Sort.Descending(a => a.UpdateTimestamp);
            FindOptions<LoanApplicationData> findOptions = new FindOptions<LoanApplicationData>()
            {
                Sort = sort
            };
            
            return (await (await GetCollection<LoanApplicationData>(settings, Constants.CollectionName.LoanApplication))
                .FindAsync(Builders<LoanApplicationData>.Filter.Empty, findOptions))                
                .ToList();
        }

        public async Task<IEnumerable<LoanApplicationData>> GetByUserId(IDataSettings settings, Guid userId)
        {
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(a => a.UserId, userId)
                ;
            return (await (await GetCollection<LoanApplicationData>(settings, Constants.CollectionName.LoanApplication))
                .FindAsync(filter))
                .ToList();
        }
    }
}
