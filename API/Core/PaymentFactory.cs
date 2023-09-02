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
            Payment payment = new Payment(data, _dataSaver, this, _transactionDataSaver);
            if (data.Transactions != null)
            {
                payment.Transactions = data.Transactions
                    .Select<PaymentTransactionData, IPaymentTransaction>(d => new PaymentTransaction(d, _transactionDataSaver))
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

        public async Task<IEnumerable<IPayment>> GetByLoanId(ISettings settings, Guid loanId)
        {
            return (await _dataFactory.GetByLoanId(new CommonCore.DataSettings(settings), loanId))
                .Select<PaymentData, IPayment>(Create);
        }
    }
}
