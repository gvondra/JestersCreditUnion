using JestersCreditUnion.Loan.Framework.Enumerations;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPaymentIntakeSaver
    {
        Task Commit(ISettings settings, PaymentIntakeStatus intakeStatusFilter, PaymentIntakeStatus intakeStatus, PaymentStatus paymentStatus, string userId);
        Task Create(ISettings settings, IPaymentIntake paymentIntake, string userId);
        Task Update(ISettings settings, IPaymentIntake paymentIntake, string userId);
    }
}
