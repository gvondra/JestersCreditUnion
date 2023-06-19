﻿using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

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

        private Loan Create(LoanData data) => new Loan(data, _dataSaver, this);

        public ILoan Create(ILoanApplication loanApplication)
        {
            return Create(
                new LoanData
                {
                    LoanId = Guid.NewGuid(),
                    Number = _numberGenerator.Generate(),
                    LoanApplicationId = loanApplication.LoanApplicationId,
                    Agreement = new LoanAgreementData
                    {
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
    }
}
