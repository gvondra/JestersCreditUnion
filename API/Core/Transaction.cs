﻿using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class Transaction : ITransaction
    {
        private readonly TransactionData _data;
        private readonly ITransactionDataSaver _dataSaver;
        private readonly ILoan _loan;

        public Transaction(TransactionData data, ITransactionDataSaver dataSaver)
            : this(data, dataSaver, null)
        { }

        public Transaction(TransactionData data, ITransactionDataSaver dataSaver, ILoan loan)
        {
            _data = data;
            _dataSaver = dataSaver;
            _loan = loan;
        }

        public Guid TransactionId => _data.TransactionId;

        public Guid LoanId => _data.LoanId;

        public DateTime Date => _data.Date;

        public TransactionType Type => (TransactionType)_data.Type;

        public decimal Amount => _data.Amount;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ITransactionHandler transactionHandler)
        {
            if (_loan == null)
                throw new ApplicationException("Loan is null. Cannot create transaction when no loan is given.");
            return _dataSaver.Create(transactionHandler, _data);
        }

    }
}