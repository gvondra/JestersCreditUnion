using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class Loan : ILoan
    {
        private readonly LoanData _data;
        private readonly ILoanDataSaver _dataSaver;
        private ILoanAgreement _agreement;

        public Loan(LoanData data,
            ILoanDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid LoanId => _data.LoanId;

        public string Number => _data.Number;

        public Guid LoanApplicationId { get => _data.LoanApplicationId; }

        public ILoanAgreement Agreement
        {
            get
            {
                if (_agreement == null)
                {
                    if (_data.Agreement == null)
                    {
                        _data.Agreement = new LoanAgreementData();
                    }
                    _agreement = new LoanAgreement(_data.Agreement);
                }
                return _agreement;
            }
        }

        public DateTime? InitialDisbursementDate { get => _data.InitialDisbursementDate; set => _data.InitialDisbursementDate = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public Task Create(ISettings settings) => _dataSaver.Create(new DataSettings(settings), _data);
    }
}
