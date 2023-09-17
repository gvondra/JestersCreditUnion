﻿using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanApplicationFactory : ILoanApplicationFactory
    {
        private readonly ILoanApplicationDataFactory _dataFactory;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILookupFactory _lookupFactory;
        private readonly IIdentificationCardDataSaver _identificationCardDataSaver;

        public LoanApplicationFactory(ILoanApplicationDataFactory dataFactory,
            ILoanApplicationDataSaver dataSaver,
            ILookupFactory lookupFactory,
            IIdentificationCardDataSaver identificationCardDataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _lookupFactory = lookupFactory;
            _identificationCardDataSaver = identificationCardDataSaver;
        }

        public IAddressFactory AddressFactory { get; set; }
        public IEmailAddressFactory EmailAddressFactory { get; set; }
        public IPhoneFactory PhoneFactory { get; set; }

        private LoanApplication Create(LoanApplicationData data) => new LoanApplication(data, _dataSaver, this, _lookupFactory, _identificationCardDataSaver);

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
            LoanApplicationData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            return data != null ? Create(data) : null;
        }

        public async Task<IEnumerable<ILoanApplication>> GetByUserId(ISettings settings, Guid userId)
        {
            return (await _dataFactory.GetByUserId(new CommonCore.DataSettings(settings), userId))
                .Select<LoanApplicationData, ILoanApplication>(Create);
        }
    }
}