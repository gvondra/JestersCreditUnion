using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;

namespace JestersCreditUnion.Core
{
    public class Loan : ILoan
    {
        private readonly LoanData _data;
        private ILoanAgreement _agreement;

        public Loan(LoanData data)
        {
            _data = data;
        }

        public Guid LoanId => _data.LoanId;

        public string Number => _data.Number;

        public Guid LoanApplicationId { get => _data.LoanApplicationId; }

        public ILoanAgreement Agreement
        {
            get
            {
                if (_agreement == null && _data.Agreement != null) 
                    _agreement = new LoanAgreement(_data.Agreement);
                return _agreement;
            }
        }

        public DateTime? InitialDisbursementDate { get => _data.InitialDisbursementDate; set => _data.InitialDisbursementDate = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;
    }
}
