using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IPaymentIntakeService
    {
        Task<List<PaymentIntake>> GetByStatuses(ISettings settings, IEnumerable<short> statuses);
        Task<PaymentIntake> Create(ISettings settings, PaymentIntake paymentIntake);
        Task<PaymentIntake> Update(ISettings settings, PaymentIntake paymentIntake);
        Task Commit(ISettings settings);
    }
}
