using JestersCreditUnion.Interface.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILoanService
    {
        Task<Loan> Create(ISettings settings, Loan loan);
    }
}
