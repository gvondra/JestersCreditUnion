using JestersCreditUnion.Loan.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPhoneDataFactory
    {
        Task<PhoneData> Get(ISqlSettings settings, Guid id);
        Task<PhoneData> Get(ISqlSettings settings, string number);
    }
}
