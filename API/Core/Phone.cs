using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
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

        public Task Create(ISettings settings) => _dataSaver.Create(new DataSettings(settings), _data);
    }
}
