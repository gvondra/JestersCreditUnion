﻿using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class PaymentSaver : IPaymentSaver
    {
        private readonly IPaymentFactory _factory;
        private readonly IPaymentDataSaver _dataSaver;

        public PaymentSaver(IPaymentFactory paymentFactory, IPaymentDataSaver dataSaver)
        {
            _factory = paymentFactory;
            _dataSaver = dataSaver;
        }

        public async Task<IEnumerable<IPayment>> Save(Framework.ISettings settings, IEnumerable<IPayment> payments)
        {
            IEnumerable<PaymentData> data = payments
                .Where(p => p is Payment)
                .Cast<Payment>()
                .GroupBy(
                p => new PaymentGroupKey { LoanId = p.LoanId, TransactionNumber = p.TransactionNumber, Date = p.Date },
                (key, elements) => ProcessPaymentGroup(elements),
                new PaymentGroupKeyComperer());
            await Saver.Save(
                new TransactionHandler(settings),
                th => _dataSaver.Save(th, data));
            return data.Select<PaymentData, IPayment>(d => new Payment(d, _dataSaver, _factory));
        }

        private static PaymentData ProcessPaymentGroup(IEnumerable<Payment> payments)
        {
            List<PaymentData> data = payments.Select(p => p.GetData()).ToList();
            decimal amount = data.Sum(p => p.Amount);
            data[0].Status = 0;
            data[0].Amount = amount;
            return data[0];
        }

        private struct PaymentGroupKey
        {
            public Guid LoanId { get; set; }
            public string TransactionNumber { get; set; }
            public DateTime Date { get; set; }
        }

        private class PaymentGroupKeyComperer : IEqualityComparer<PaymentGroupKey>
        {
            public bool Equals(PaymentGroupKey x, PaymentGroupKey y)
            {
                return x.LoanId == y.LoanId
                    && string.Equals(x.TransactionNumber ?? string.Empty, y.TransactionNumber ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                    && x.Date == y.Date;
            }

            public int GetHashCode([DisallowNull] PaymentGroupKey obj)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "\"{0:N}\",\"{1}\",\"{2:O}\"",
                    obj.LoanId,
                    obj.TransactionNumber.Replace("\"", "\"\""),
                    obj.Date)
                    .GetHashCode();
            }
        }
    }
}
