﻿using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface ILoanApplicationService
    {
        Task<LoanApplication> Get(ISettings settings, Guid id);
        Task<LoanApplication> Create(ISettings settings, LoanApplication loanApplication);
        Task<LoanApplication> Update(ISettings settings, LoanApplication loanApplication);
        Task<LoanApplicationComment> AppendComent(ISettings settings, Guid id, LoanApplicationComment comment, bool isPublic = false);
        Task Deny(ISettings settings, Guid id, LoanApplicationDenial denial);
    }
}
