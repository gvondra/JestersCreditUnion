using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanDisburser : ILoanDisburser
    {
        public async Task<ITransaction> Disburse(ISettings settings, ILoan loan)
        {
            ITransaction transaction = loan.CreateTransaction(DateTime.Today, TransactionType.Disbursement, loan.Agreement.OriginalAmount * -1);
            loan.InitialDisbursementDate = DateTime.Today;
            if (!loan.FirstPaymentDue.HasValue)
            {
                loan.FirstPaymentDue = PaymentDateCalculator.FirstPaymentDate(loan.Agreement.PaymentFrequency);
                loan.NextPaymentDue = loan.FirstPaymentDue;
            }
            await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), async th =>
            {
                await loan.Update(th);
                await transaction.Create(th);
            });
            return transaction;
        }
    }
}
