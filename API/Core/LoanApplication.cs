using JestersCreditUnion.Data;
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
    public class LoanApplication : ILoanApplication
    {
        private readonly LoanApplicationData _data;
        private readonly ILoanApplicationDataSaver _dataSaver;

        public LoanApplication(LoanApplicationData data, ILoanApplicationDataSaver dataSaver)
        {
            _data = data;
            _dataSaver = dataSaver;
        }

        public Guid LoanApplicationId => _data.LoanApplicationId;

        public Guid UserId => _data.UserId;

        public LoanApplicationStatus Status { get => (LoanApplicationStatus)_data.Status; set => _data.Status = (short)value; }
        public string BorrowerName { get => _data.BorrowerName; set => _data.BorrowerName = value; }
        public DateTime BorrowerBirthDate { get => _data.BorrowerBirthDate; set => _data.BorrowerBirthDate = value; }
        public Guid? BorrowerAddressId { get => _data.BorrowerAddressId; set => _data.BorrowerAddressId = value; }
        public Guid? BorrowerEmailAddressId { get => _data.BorrowerEmailAddressId; set => _data.BorrowerEmailAddressId = value; }
        public Guid? BorrowerPhoneId { get => _data.BorrowerPhoneId; set => _data.BorrowerPhoneId = value; }
        public string BorrowerEmployerName { get => _data.BorrowerEmployerName; set => _data.BorrowerEmployerName = value; }
        public DateTime? BorrowerEmploymentHireDate { get => _data.BorrowerEmploymentHireDate; set => _data.BorrowerEmploymentHireDate = value; }
        public decimal? BorrowerIncome { get => _data.BorrowerIncome; set => _data.BorrowerIncome = value; }
        public string CoBorrowerName { get => _data.CoBorrowerName; set => _data.CoBorrowerName = value; }
        public DateTime? CoBorrowerBirthDate { get => _data.CoBorrowerBirthDate; set => _data.CoBorrowerBirthDate = value; }
        public Guid? CoBorrowerAddressId { get => _data.CoBorrowerAddressId; set => _data.CoBorrowerAddressId = value; }
        public Guid? CoBorrowerEmailAddressId { get => _data.CoBorrowerEmailAddressId; set => _data.CoBorrowerEmailAddressId = value; }
        public Guid? CoBorrowerPhoneId { get => _data.CoBorrowerPhoneId; set => _data.CoBorrowerPhoneId = value; }
        public string CoBorrowerEmployerName { get => _data.CoBorrowerEmployerName; set => _data.CoBorrowerEmployerName = value; }
        public DateTime? CoBorrowerEmploymentHireDate { get => _data.CoBorrowerEmploymentHireDate; set => _data.CoBorrowerEmploymentHireDate = value; }
        public decimal? CoBorrowerIncome { get => _data.CoBorrowerIncome; set => _data.CoBorrowerIncome = value; }
        public decimal Amount { get => _data.Amount; set => _data.Amount = value; }
        public string Purpose { get => _data.Purpose; set => _data.Purpose = value; }
        public decimal? MortgagePayment { get => _data.MortgagePayment; set => _data.MortgagePayment = value; }
        public decimal? RentPayment { get => _data.RentPayment; set => _data.RentPayment = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public Task Create(ISettings settings) => _dataSaver.Create(new DataSettings(settings), _data);

        public Task Update(ISettings settings) => _dataSaver.Update(new DataSettings(settings), _data);
    }
}
