﻿using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanDataSaver
    {
        ILoanAgreementDataSaver LoanAgrementDataSaver { get; }
        Task Create(ISqlTransactionHandler transactionHandler, LoanData data);
        Task Update(ISqlTransactionHandler transactionHandler, LoanData data);
    }
}
