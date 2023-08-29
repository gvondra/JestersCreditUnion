using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PaymentTransaction : Transaction, IPaymentTransaction
    {
        private readonly PaymentTransactionData _data;

        public PaymentTransaction(PaymentTransactionData data, ITransactionDataSaver dataSaver)
            : base(data, dataSaver)
        {
            _data = data;
        }

        public Guid PaymentId => _data.PaymentId;

        public short TermNumber { get => _data.TermNumber; set => _data.TermNumber = value; }

        public Task Create(ITransactionHandler transactionHandler) => Create(transactionHandler, PaymentId, TermNumber);
    }
}
