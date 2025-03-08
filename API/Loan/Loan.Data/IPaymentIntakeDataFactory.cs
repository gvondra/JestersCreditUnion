using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentIntakeDataFactory
    {
        Task<PaymentIntakeData> Get(ISettings settings, Guid id);
        Task<IEnumerable<PaymentIntakeData>> GetByStatuses(ISettings settings, IEnumerable<short> statuses);
    }
}
