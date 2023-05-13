using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanAgreement : ILoanAgreement
    {
        private readonly LoanAgreementData _data;

        public LoanAgreement(LoanAgreementData data)
        {
            _data = data;
        }

        public LoanAgrementStatus Status { get => (LoanAgrementStatus)_data.Status; set => _data.Status = (short)value; }

        public DateTime CreateDate => _data.CreateDate;

        public DateTime? AgreementDate { get => _data.AgreementDate; set => _data.AgreementDate = value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        public Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        public Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        public Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        public Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        public Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        public Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public decimal OriginalAmount { get => _data.OriginalAmount; set => _data.OriginalAmount = value; }
        public short OriginalTerm { get => _data.OriginalTerm; set => _data.OriginalTerm = value; }
        public decimal InterestRate { get => _data.InterestRate; set => _data.InterestRate = value; }
        public decimal PaymentAmount { get => _data.PaymentAmount; set => _data.PaymentAmount = value; }
        public short PaymentFrequency { get => _data.PaymentFrequency; set => _data.PaymentFrequency = value; }
    }
}
