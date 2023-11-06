using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public interface IDataPurger
    {
        Task Purge(DbConnection connection, string tableName);
    }
}
