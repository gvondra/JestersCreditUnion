using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;

namespace JestersCreditUnion.Core
{
    public class Payment : IPayment
    {
        private readonly PaymentData _data;

        public Payment(PaymentData data)
        {
            _data = data;
        }

        public Guid PaymentId => _data.PaymentId;

        public string LoanNumber => _data.LoanNumber;

        public string TransactionNumber => _data.TransactionNumber;

        public DateTime Date => _data.Date;

        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public PaymentStatus Status { get => (PaymentStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        internal PaymentData GetData() => _data;
    }
}
