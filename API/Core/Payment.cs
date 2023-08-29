using JestersCreditUnion.CommonCore;
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
    public class Payment : IPayment
    {
        private readonly PaymentData _data;
        private readonly IPaymentDataSaver _dataSaver;
        private readonly IPaymentFactory _factory;

        public Payment(PaymentData data, IPaymentDataSaver dataSaver, IPaymentFactory factory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = factory;
        }

        public Guid PaymentId => _data.PaymentId;

        public Guid LoanId => _data.LoanId;

        public string TransactionNumber => _data.TransactionNumber;

        public DateTime Date => _data.Date;

        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public PaymentStatus Status { get => (PaymentStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public List<ITransaction> Transactions { get; internal set; }

        public Task<ILoan> GetLoan(Framework.ISettings settings) => _factory.LoanFactory.Get(settings, LoanId);

        public async Task Update(ITransactionHandler transactionHandler)
        {
            await _dataSaver.Update(transactionHandler, _data);
            if (Transactions != null)
            {
                foreach (ITransaction transaction in Transactions.Where(t => t.IsNew))
                {
                    await transaction.Create(transactionHandler, PaymentId);
                }
            }
        }

        internal PaymentData GetData() => _data;
    }
}
