using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class PaymentIntakeSaver : IPaymentIntakeSaver
    {
        private readonly IPaymentIntakeDataSaver _dataSaver;

        public PaymentIntakeSaver(IPaymentIntakeDataSaver dataSaver)
        {
            _dataSaver = dataSaver;
        }

        public Task Commit(
            Framework.ISettings settings,
            PaymentIntakeStatus intakeStatusFilter,
            DateTime maxTimestamp,
            PaymentIntakeStatus intakeStatus,
            PaymentStatus paymentStatus,
            string userId)
            => Saver.Save(new TransactionHandler(settings), (th) => _dataSaver.Commit(th, (short)intakeStatusFilter, maxTimestamp, (short)intakeStatus, (short)paymentStatus, userId));

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
