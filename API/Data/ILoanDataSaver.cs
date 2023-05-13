using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataSaver
    {
        Task Create(IDataSettings settings, LoanData data);
    }
}
