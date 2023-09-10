using JestersCreditUnion.CommonCore;
using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class Phone : IPhone
    {
        private readonly PhoneData _data;
        private readonly IPhoneDataSaver _dataSaver;

        public Phone(PhoneData data, IPhoneDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid PhoneId => _data.PhoneId;

        public string Number => _data.Number ?? string.Empty;

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public Task Create(ITransactionHandler transactionHandler) => _dataSaver.Create(transactionHandler, _data);
    }
}
