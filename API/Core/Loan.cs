﻿using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class Loan : ILoan
    {
        private readonly LoanData _data;
        private readonly ILoanDataSaver _dataSaver;
        private readonly ILoanFactory _factory;
        private ILoanAgreement _agreement;

        public Loan(LoanData data,
            ILoanDataSaver dataSaver,
            ILoanFactory factory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = factory;
        }

        public Guid LoanId => _data.LoanId;

        public string Number => _data.Number;

        public Guid LoanApplicationId => _data.LoanApplicationId;

        public ILoanAgreement Agreement
        {
            get
            {
                if (_agreement == null)
                {
                    if (_data.Agreement == null)
                    {
                        _data.Agreement = new LoanAgreementData();
                    }
                    _agreement = new LoanAgreement(_data.Agreement, _dataSaver.LoanAgrementDataSaver, _factory, this);
                }
                return _agreement;
            }
        }

        public DateTime? InitialDisbursementDate { get => _data.InitialDisbursementDate; set => _data.InitialDisbursementDate = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public DateTime? FirstPaymentDue { get => _data.FirstPaymentDue; set => _data.FirstPaymentDue = value; }
        public DateTime? NextPaymentDue { get => _data.NextPaymentDue; set => _data.NextPaymentDue = value; }

        public async Task Create(ITransactionHandler transactionHandler)
        {
            if (_agreement == null)
                throw new ApplicationException("Cannot create loan. No loan agreement set");
            await _agreement.Create(transactionHandler);
            await _dataSaver.Create(transactionHandler, _data);
        }

        public async Task Update(ITransactionHandler transactionHandler)
        {
            await _dataSaver.Update(transactionHandler, _data);
            if (_agreement != null)
                await _agreement.Update(transactionHandler);
        }

        public Task<IEnumerable<ITransaction>> GetTransactions(Framework.ISettings settings) => _factory.TransactionFacatory.GetByLoanId(settings, LoanId);
        public ITransaction CreateTransaction(DateTime date, TransactionType type, decimal amount) => _factory.TransactionFacatory.Create(this, date, type, amount);
    }
}
