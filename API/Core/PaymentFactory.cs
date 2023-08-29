using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IPaymentDataFactory _dataFactory;
        private readonly IPaymentDataSaver _dataSaver;
        private readonly ITransactionDataSaver _transactionDataSaver;

        public PaymentFactory(
            IPaymentDataFactory dataFactory,
            ITransactionDataSaver transactionDataSaver,
            IPaymentDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _transactionDataSaver = transactionDataSaver;
            _dataSaver = dataSaver;
        }

        private Payment Create(PaymentData data)
        {
            Payment payment = new Payment(data, _dataSaver);
            if (data.Transactions != null)
            {
                payment.Transactions = data.Transactions
                    .Select<TransactionData, ITransaction>(d => new Transaction(d, _transactionDataSaver))
                    .ToList();
            }
            return payment;
        }

        public IPayment Create(string loanNumber,
            string transactionNumber,
            DateTime date)
        {
            if (string.IsNullOrEmpty(loanNumber))
                throw new ArgumentNullException(nameof(loanNumber));
            if (string.IsNullOrEmpty(transactionNumber))
                throw new ArgumentNullException(nameof(transactionNumber));
            Payment payment = Create(new PaymentData
            {
                LoanNumber = loanNumber,
                TransactionNumber = transactionNumber,
                Date = date
            });
            payment.Status = PaymentStatus.Unprocessed;
            return payment;
        }

        public async Task<IEnumerable<IPayment>> GetByStatus(ISettings settings, PaymentStatus status)
        {
            return (await _dataFactory.GetByStatus(new CommonCore.DataSettings(settings), (short)status))
                .Select<PaymentData, IPayment>(Create);
        }
    }
}
