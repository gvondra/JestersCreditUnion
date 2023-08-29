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

        public Payment(PaymentData data, IPaymentDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid PaymentId => _data.PaymentId;

        public string LoanNumber => _data.LoanNumber;

        public string TransactionNumber => _data.TransactionNumber;

        public DateTime Date => _data.Date;

        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public PaymentStatus Status { get => (PaymentStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public List<ITransaction> Transactions { get; internal set; }

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
