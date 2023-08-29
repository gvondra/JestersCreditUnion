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

        public ILoanFactory LoanFactory { get; set; }

        private Payment Create(PaymentData data)
        {
            Payment payment = new Payment(data, _dataSaver, this);
            if (data.Transactions != null)
            {
                payment.Transactions = data.Transactions
                    .Select<TransactionData, ITransaction>(d => new Transaction(d, _transactionDataSaver))
                    .ToList();
            }
            return payment;
        }

        public IPayment Create(
            ILoan loan,
            string transactionNumber,
            DateTime date)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));
            if (string.IsNullOrEmpty(transactionNumber))
                throw new ArgumentNullException(nameof(transactionNumber));
            Payment payment = Create(
                new PaymentData
                {
                    Date = date,
                    LoanId = loan.LoanId,
                    TransactionNumber = transactionNumber
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
