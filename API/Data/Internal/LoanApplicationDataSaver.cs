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

        public async Task AppendComment(IDataSettings settings, Guid id, LoanApplicationCommentData data)
        {            
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(la => la.LoanApplicationId, id)
                ;
            UpdateDefinition<LoanApplicationData> update = Builders<LoanApplicationData>.Update
                .Set(la => la.UpdateTimestamp, DateTime.UtcNow)
                .Push(la => la.Comments, data);
            await(await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.LoanApplication)
                .UpdateOneAsync(filter, update);
            ;
        }

        public async Task SetDenial(IDataSettings settings, Guid id, short loanApplicationStatus, LoanApplicationDenialData denial)
        {
            FilterDefinition<LoanApplicationData> filter = Builders<LoanApplicationData>.Filter
                .Eq(la => la.LoanApplicationId, id)
                ;
            UpdateDefinition<LoanApplicationData> update = Builders<LoanApplicationData>.Update
                .Set(la => la.UpdateTimestamp, DateTime.UtcNow)
                .Set(la => la.Status, loanApplicationStatus)
                .Set(la => la.Denial, denial);
            await (await _mongoClientFactory.GetDatabase(settings))
                .GetCollection<LoanApplicationData>(Constants.CollectionName.LoanApplication)
                .UpdateOneAsync(filter, update);
            ;
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
