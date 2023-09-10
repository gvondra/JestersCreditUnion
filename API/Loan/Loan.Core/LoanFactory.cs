using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanFactory : ILoanFactory
    {
        private readonly ILoanDataFactory _dataFactory;
        private readonly ILoanDataSaver _dataSaver;
        private readonly LoanNumberGenerator _numberGenerator;
        private readonly IPaymentFactory _paymentFactory;
        private readonly ILookupFactory _lookupFactory;

        public LoanFactory(ILoanDataFactory dataFactory,
            ILoanDataSaver dataSaver,
            LoanNumberGenerator numberGenerator,
            IPaymentDataFactory paymentDataFactory,
            IPaymentDataSaver paymentDataSaver,
            ITransactionDataSaver transactionDataSaver,
            ILookupFactory lookupFactory)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _numberGenerator = numberGenerator;
            _paymentFactory = new PaymentFactory(paymentDataFactory, transactionDataSaver, paymentDataSaver);
            _lookupFactory = lookupFactory;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }
        public ITransactionFacatory TransactionFacatory { get; set; }

        private Loan Create(LoanData data) => new Loan(data, _dataSaver, this, _paymentFactory, _lookupFactory);

        public ILoan Create(ILoanApplication loanApplication)
        {
            Guid loanId = Guid.NewGuid();
            return Create(
                new LoanData
                {
                    LoanId = loanId,
                    Number = _numberGenerator.Generate(),
                    LoanApplicationId = loanApplication.LoanApplicationId,
                    Agreement = new LoanAgreementData
                    {
                        LoanId = loanId,
                        CreateDate = DateTime.Today
                    }
                });
        }

        public async Task<ILoan> GetByNumber(ISettings settings, string number)
        {
            Loan result = null;
            LoanData data = await _dataFactory.GetByNumber(new CommonCore.DataSettings(settings), number);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<ILoan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            Loan result = null;
            LoanData data = await _dataFactory.GetByLoanApplicationId(new CommonCore.DataSettings(settings), loanApplicationId);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<ILoan> Get(ISettings settings, Guid id)
        {
            Loan result = null;
            LoanData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            if (data != null)
                result = Create(data);
            return result;
        }

        public async Task<IEnumerable<ILoan>> GetByNameBirthDate(ISettings settings, string name, DateTime birthDate)
        {
            return (await _dataFactory.GetByNameBirthDate(new CommonCore.DataSettings(settings), name, birthDate))
                .Select<LoanData, ILoan>(Create);
        }

        public async Task<IEnumerable<ILoan>> GetWithUnprocessedPayments(ISettings settings)
        {
            return (await _dataFactory.GetWithUnprocessedPayments(new CommonCore.DataSettings(settings)))
                .Select<LoanData, ILoan>(Create);
        }
    }
}
