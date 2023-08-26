using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PaymentSaver : IPaymentSaver
    {
        private readonly IPaymentDataSaver _dataSaver;

        public PaymentSaver(IPaymentDataSaver dataSaver)
        {
            _dataSaver = dataSaver;
        }

        public async Task Save(Framework.ISettings settings, IEnumerable<IPayment> payments)
        {
            IEnumerable<PaymentData> data = payments
                .Where(p => p is Payment)
                .Cast<Payment>()
                .Select<Payment, PaymentData>(p => p.GetData());
            await Saver.Save(
                new TransactionHandler(settings),
                th => _dataSaver.Save(th, data));
        }
    }
}
