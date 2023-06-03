using JestersCreditUnion.Data.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class LoanDataFactory : BaseDataFactory, ILoanDataFactory
    {
        private readonly IMongoClientFactory _mongoClientFactory;

        public LoanDataFactory(IMongoClientFactory mongoClientFactory)
            : base(mongoClientFactory) 
        {
            _mongoClientFactory = mongoClientFactory;
        }

        public async Task<LoanData> Get(IDataSettings settings, Guid id)
        {
            FilterDefinition<LoanData> filter = Builders<LoanData>.Filter
                .Eq(ln => ln.LoanId, id)
                ;
            return (await(await GetCollection<LoanData>(settings, Constants.CollectionName.Loan))
                .FindAsync(filter))
                .FirstOrDefault()
                ;
        }

        public async Task<LoanData> GetByLoanApplicationId(IDataSettings settings, Guid loanApplicationId)
        {
            FilterDefinition<LoanData> filter = Builders<LoanData>.Filter
                .Eq(ln => ln.LoanApplicationId, loanApplicationId)
                ;
            return (await(await GetCollection<LoanData>(settings, Constants.CollectionName.Loan))
                .FindAsync(filter))
                .FirstOrDefault()
                ;
        }

        public async Task<LoanData> GetByNumber(IDataSettings settings, string number)
        {
            FilterDefinition<LoanData> filter = Builders<LoanData>.Filter
                .Eq(ln => ln.Number, number)
                ;
            return (await(await GetCollection<LoanData>(settings, Constants.CollectionName.Loan))
                .FindAsync(filter))
                .FirstOrDefault()
                ;
        }
    }
}
