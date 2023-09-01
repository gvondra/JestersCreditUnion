﻿using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    internal class IdentificationCard
    {
        private readonly IdentificationCardData _data;
        private readonly IIdentificationCardDataSaver _dataSaver;

        internal IdentificationCard(
            IdentificationCardData data,
            IIdentificationCardDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid IdentificationCardId => _data.IdentificationCardId;
        public byte[] InitializationVector { get => _data.InitializationVector; set => _data.InitializationVector = value; }
        public byte[] Key { get => _data.Key; set => _data.Key = value; }
        public DateTime CreateTimestamp => _data.CreateTimestamp;
        public DateTime UpdateTimestamp => _data.UpdateTimestamp;
        public bool IsNew => _data.Manager.GetState(_data) == BrassLoon.DataClient.DataState.New;

        public Task Create(ITransactionHandler transactionHandler) => _dataSaver.Create(transactionHandler, _data);
        public Task Update(ITransactionHandler transactionHandler) => _dataSaver.Update(transactionHandler, _data);
    }
}
