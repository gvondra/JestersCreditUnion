using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class PaymentIntakeSaver : IPaymentIntakeSaver
    {
        public Task Create(Framework.ISettings settings, IPaymentIntake paymentIntake, string userId)
        {
            return Saver.Save(
                new TransactionHandler(settings),
                (th) => paymentIntake.Create(th, userId));
        }

        public Task Update(Framework.ISettings settings, IPaymentIntake paymentIntake, string userId)
        {
            return Saver.Save(
                new TransactionHandler(settings),
                (th) => paymentIntake.Update(th, userId));
        }
    }
}
