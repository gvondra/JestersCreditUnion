using JestersCreditUnion.Loan.Data;
using JestersCreditUnion.Loan.Data.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class TransactionFactory : ITransactionFacatory
    {
        private readonly ITransactionDataFactory _dataFactory;
        private readonly ITransactionDataSaver _dataSaver;

        public TransactionFactory(ITransactionDataFactory dataFactory, ITransactionDataSaver dataSaver)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
        }

        private Transaction Create(TransactionData data) => new Transaction(data, _dataSaver);
        private Transaction Create(TransactionData data, ILoan loan) => new Transaction(data, _dataSaver, loan);

        public ITransaction Create(ILoan loan, DateTime date, TransactionType type, decimal amount)
        {
            return Create(
                new TransactionData
                {
                    Date = date,
                    Type = (short)type,
                    Amount = amount
                },
                loan);
        }

        public async Task<IEnumerable<ITransaction>> GetByLoanId(ISettings settings, Guid loanId)
        {
            return (await _dataFactory.GetByLoanId(new CommonCore.DataSettings(settings), loanId))
                .Select<TransactionData, ITransaction>(Create);
        }
    }
}
