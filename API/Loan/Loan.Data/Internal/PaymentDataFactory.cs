using JestersCreditUnion.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data.Internal
{
    public class PaymentDataFactory : DataFactoryBase<PaymentData>, IPaymentDataFactory
    {
        public PaymentDataFactory(IDbProviderFactory providerFactory)
            : base(providerFactory) { }

        public async Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISqlSettings settings, DateTime date, Guid loanId, string transactionNumber)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "date", DbType.Date, DataUtil.GetParameterValue(date)),
                DataUtil.CreateParameter(_providerFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId)),
                DataUtil.CreateParameter(_providerFactory, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(transactionNumber))
            };
            PaymentData payment = null;
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetPayment_by_Date_LoanId_TransactionNumber]",
                CommandType.StoredProcedure,
                parameters,
                async reader =>
                {
                    payment = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
                    if (payment != null)
                    {
                        await reader.NextResultAsync();
                        GenericDataFactory<PaymentTransactionData> transactionDataFactory = new GenericDataFactory<PaymentTransactionData>();
                        payment.Transactions = (await transactionDataFactory.LoadData(reader, () => new PaymentTransactionData(), DataUtil.AssignDataStateManager)).ToList();
                    }
                });
            return payment;
        }

        public async Task<IEnumerable<PaymentData>> GetByLoanId(ISqlSettings settings, Guid loanId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId))
            };
            List<PaymentData> payments = new List<PaymentData>();
            List<PaymentTransactionData> transactions = new List<PaymentTransactionData>();
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            await dataReaderProcess.Read(
                settings,
                _providerFactory,
                "[ln].[GetPayment_by_LoanId]",
                CommandType.StoredProcedure,
                parameters,
                async reader =>
                {
                    payments = (await _genericDataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).ToList();

                    await reader.NextResultAsync();
                    GenericDataFactory<PaymentTransactionData> transactionDataFactory = new GenericDataFactory<PaymentTransactionData>();
                    transactions = (await transactionDataFactory.LoadData(reader, () => new PaymentTransactionData(), DataUtil.AssignDataStateManager)).ToList();
                });
            return payments.GroupJoin(transactions,
                pmt => pmt.PaymentId,
                trn => trn.PaymentId,
                (pmt, trns) =>
                {
                    pmt.Transactions = trns.ToList<PaymentTransactionData>();
                    return pmt;
                });
        }
    }
}
