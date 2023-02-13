using JestersCreditUnion.Data;
using JestersCreditUnion.Data.Models;
using JestersCreditUnion.Framework;
using System;

namespace JestersCreditUnion.Core
{
    public class LoanApplicationFactory : ILoanApplicationFactory
    {
        private readonly ILoanApplicationDataSaver _dataSaver;

        public LoanApplicationFactory(ILoanApplicationDataSaver dataSaver)
        {
            _dataSaver = dataSaver;
        }

        private LoanApplication Create(LoanApplicationData data) => new LoanApplication(data, _dataSaver);

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
    }
}
