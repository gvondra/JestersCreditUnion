using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class PaymentIntake : IPaymentIntake
    {
        private readonly PaymentIntakeData _data;
        private readonly IPaymentIntakeDataSaver _dataSaver;
        private readonly IPaymentIntakeFactory _factory;
        private ILoan _loan;

        public PaymentIntake(PaymentIntakeData data, IPaymentIntakeDataSaver dataSaver, IPaymentIntakeFactory paymentIntakeFactory, ILoan loan)
        {
            _data = data;
            _dataSaver = dataSaver;
            _factory = paymentIntakeFactory;
            _loan = loan;
        }

        public PaymentIntake(PaymentIntakeData data, IPaymentIntakeDataSaver dataSaver, IPaymentIntakeFactory paymentIntakeFactory)
            : this(data, dataSaver, paymentIntakeFactory, null)
        { }

        public Guid PaymentIntakeId => _data.PaymentIntakeId;
        private Guid LoanId { get => _data.LoanId; set => _data.LoanId = value; }

        public string TransactionNumber { get => _data.TransactionNumber; set => _data.TransactionNumber = value; }
        public DateTime Date { get => _data.Date; set => _data.Date = value; }
        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public short Status { get => _data.Status; set => _data.Status = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public string CreateUserId => _data.CreateUserId;

        public string UpdateUserId => _data.UpdateUserId;

        public Task Create(ITransactionHandler transactionHandler, string userId)
        {
            if (_loan == null)
                throw new ApplicationException("Loan must be set in order to create a payment intake");
            LoanId = _loan.LoanId;
            return _dataSaver.Create(transactionHandler, _data, userId);
        }

        public Task Update(ITransactionHandler transactionHandler, string userId) => _dataSaver.Update(transactionHandler, _data, userId);

        public async Task<ILoan> GetLaon(Framework.ISettings settings)
        {
            if (_loan == null)
            {
                _loan = await _factory.LoanFactory.Get(settings, LoanId);
            }
            return _loan;
        }
    }
}
