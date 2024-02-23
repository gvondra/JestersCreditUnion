using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
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

        public Guid LoanId { get => _data.LoanId; private set => _data.LoanId = value; }

        public DateTime Date => _data.Date;

        public TransactionType Type => (TransactionType)_data.Type;

        public decimal Amount => _data.Amount;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public bool IsNew => _data.Manager.GetState(_data) == BrassLoon.DataClient.DataState.New;

        public virtual Task Create(ITransactionHandler transactionHandler, Guid? paymentId = null, short? termNumber = null)
        {
            if (_loan == null)
                throw new ApplicationException("Loan is null. Cannot create transaction when no loan is given.");
            LoanId = _loan.LoanId;
            return _dataSaver.Create(transactionHandler, _data, paymentId, termNumber);
        }
    }
}
