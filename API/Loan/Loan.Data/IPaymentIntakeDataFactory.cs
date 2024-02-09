using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data
{
    public interface IPaymentIntakeDataFactory
    {
        Task<IEnumerable<PaymentIntakeData>> GetByStatuses(ISqlSettings settings, IEnumerable<short> statuses);
    }
}
