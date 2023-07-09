﻿using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanFactory : ILoanFactory
    {
        private readonly ILoanDataFactory _dataFactory;
        private readonly ILoanDataSaver _dataSaver;
        private readonly LoanNumberGenerator _numberGenerator;

        public LoanFactory(ILoanDataFactory dataFactory,
            ILoanDataSaver dataSaver,
            LoanNumberGenerator numberGenerator)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _numberGenerator = numberGenerator;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }
        public ITransactionFacatory TransactionFacatory { get; set; }

        private Loan Create(LoanData data) => new Loan(data, _dataSaver, this);

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
    }
}
