using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataFactory
    {
        Task<LoanData> GetByNumber(IDataSettings settings, string number);
    }
}
