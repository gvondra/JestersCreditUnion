﻿using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class Address : IAddress
    {
        private readonly AddressData _data;
        private readonly IAddressDataSaver _dataSaver;

        public Address(AddressData data, IAddressDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid AddressId => _data.AddressId;

        public byte[] Hash => _data.Hash;

        public string Recipient => _data.Recipient ?? string.Empty;

        public string Attention => _data.Attention ?? string.Empty;

        public string Delivery => _data.Delivery ?? string.Empty;

        public string Secondary => _data.Secondary ?? string.Empty;

        public string City => _data.City ?? string.Empty;

        public string State => (_data.State ?? string.Empty).Trim().ToUpper(CultureInfo.InvariantCulture);

        public string PostalCode => _data.PostalCode ?? string.Empty;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ITransactionHandler transactionHandler) => _dataSaver.Create(transactionHandler, _data);
    }
}