using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanApplicationDenial : ILoanApplicationDenial
    {
        private readonly LoanApplicationDenialData _data;
        private readonly ILoanApplicationDataSaver _dataSaver;
        private readonly ILookupFactory _lookupFactory;

        public LoanApplicationDenial(LoanApplicationDenialData data,
            ILoanApplicationDataSaver dataSaver,
            ILookupFactory lookupFactory)
        {
            _data = data;
            _dataSaver = dataSaver;
            _lookupFactory = lookupFactory;
        }

        public LoanApplicationDenialReason Reason { get => (LoanApplicationDenialReason)_data.Reason; set => _data.Reason = (short)value; }
        public DateTime Date { get => _data.Date; set => _data.Date = value.Date; }
        public Guid UserId { get => _data.UserId; set => _data.UserId = value; }
        public string Text { get => _data.Text; set => _data.Text = (value ?? string.Empty).Trim(); }

        public Task Save(CommonCore.ITransactionHandler transactionHandler, Guid id, LoanApplicationStatus status) => _dataSaver.SetDenial(transactionHandler, id, (short)status, _data);

        public async Task<string> GetReasonDescription(ISettings settings)
        {
            string result = null;
            ILookup lookup = await _lookupFactory.GetLookup(settings, typeof(LoanApplicationStatus));
            if (lookup != null)
            {
                result = lookup.GetDataValue(Reason);
            }
            return result;
        }
    }
}
