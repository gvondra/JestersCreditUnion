using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class Loan : ILoan
    {
        private readonly LoanData _data;
        private readonly ILoanDataSaver _dataSaver;
        private readonly ILoanFactory _factory;
        private readonly IPaymentFactory _paymentFactory;
        private readonly ILookupFactory _lookupFactory;
        private ILoanAgreement _agreement;

        public Loan(LoanData data,
            ILoanDataSaver dataSaver,
            ILoanFactory factory,
            IPaymentFactory paymentFactory,
            ILookupFactory lookupFactory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = factory;
            _paymentFactory = paymentFactory;
            _lookupFactory = lookupFactory;
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
        public LoanStatus Status { get => (LoanStatus)_data.Status; set => _data.Status = (short)value; }
        public decimal? Balance { get => _data.Balance; set => _data.Balance = value; }

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
        public Task<IEnumerable<IPayment>> GetPayments(Framework.ISettings settings) => _paymentFactory.GetByLoanId(settings, LoanId);
        public async Task<string> GetStatusDescription(Framework.ISettings settings)
        {
            string result = null;
            ILookup lookup = await _lookupFactory.GetLookup(settings, typeof(LoanStatus));
            if (lookup != null)
            {
                result = lookup.GetDataValue(Status);
            }
            return result;
        }
    }
}
