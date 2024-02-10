using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class PaymentIntakeFactory : IPaymentIntakeFactory
    {
        private readonly IPaymentIntakeDataFactory _dataFactory;
        private readonly IPaymentIntakeDataSaver _dataSaver;
        private readonly ILoanFactory _loanFactory;

        public PaymentIntakeFactory(IPaymentIntakeDataFactory dataFactory, IPaymentIntakeDataSaver dataSaver, ILoanFactory loanFactory)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _loanFactory = loanFactory;
        }

        public ILoanFactory LoanFactory => _loanFactory;

        private PaymentIntake Create(PaymentIntakeData data, ILoan loan) => new PaymentIntake(data, _dataSaver, this, loan);
        private PaymentIntake Create(PaymentIntakeData data) => new PaymentIntake(data, _dataSaver, this);

        public IPaymentIntake Create(ILoan loan) => Create(new PaymentIntakeData(), loan);

        public async Task<IEnumerable<IPaymentIntake>> GetByStatues(Framework.ISettings settings, IEnumerable<PaymentIntakeStatus> statuses)
        {
            return (await _dataFactory.GetByStatuses(new DataSettings(settings), statuses.Cast<short>()))
                .Select<PaymentIntakeData, IPaymentIntake>(Create);
        }

        public async Task<IPaymentIntake> Get(Framework.ISettings settings, Guid id)
        {
            PaymentIntakeData data = await _dataFactory.Get(new DataSettings(settings), id);
            return data != null ? Create(data) : null;
        }
    }
}
