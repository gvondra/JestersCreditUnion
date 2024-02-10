using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPaymentIntakeSaver
    {
        Task Create(ISettings settings, IPaymentIntake paymentIntake, string userId);
        Task Update(ISettings settings, IPaymentIntake paymentIntake, string userId);
    }
}
