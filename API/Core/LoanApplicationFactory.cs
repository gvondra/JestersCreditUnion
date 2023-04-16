﻿using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanApplicationFactory : ILoanApplicationFactory
    {
        private readonly ILoanApplicationDataFactory _dataFactory;
        private readonly ILoanApplicationDataSaver _dataSaver;

        public LoanApplicationFactory(ILoanApplicationDataFactory dataFactory,
            ILoanApplicationDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }

        private LoanApplication Create(LoanApplicationData data) => new LoanApplication(data, _dataSaver, this);

        public ILoanApplication Create(Guid userId)
        {
            return Create(
                new LoanApplicationData
                {
                    LoanApplicationId = Guid.NewGuid(),
                    UserId = userId,
                    ApplicationDate = DateTime.Today
                });
        }

        public async Task<ILoanApplication> Get(ISettings settings, Guid id)
        {
            LoanApplicationData data = await _dataFactory.Get(new DataSettings(settings), id);
            return data != null ? Create(data) : null;
        }
    }
}
