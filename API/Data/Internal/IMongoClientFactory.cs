using MongoDB.Driver;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public interface IMongoClientFactory
    {
        Task<MongoClient> CreateClient(IDataSettings dataSettings);
        Task<IMongoDatabase> GetDatabase(IDataSettings dataSettings);
    }
}
