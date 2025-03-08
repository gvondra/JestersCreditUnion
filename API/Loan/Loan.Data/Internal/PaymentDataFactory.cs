using JestersCreditUnion.Loan.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Data.Internal
{
    public class PaymentDataFactory : DataFactoryBase<PaymentData>, IPaymentDataFactory
    {
        public PaymentDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<PaymentData> dataFactory)
            : base(providerFactory, dataFactory) { }

        public async Task<PaymentData> GetByDateLoanNumberTransactionNumber(ISettings settings, DateTime date, Guid loanId, string transactionNumber)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "date", DbType.Date, DataUtil.GetParameterValue(date)),
                DataUtil.CreateParameter(ProviderFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId)),
                DataUtil.CreateParameter(ProviderFactory, "transactionNumber", DbType.AnsiString, DataUtil.GetParameterValue(transactionNumber))
            };
            PaymentData payment = null;
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                "[ln].[GetPayment_by_Date_LoanId_TransactionNumber]",
                CommandType.StoredProcedure,
                parameters,
                async reader =>
                {
                    payment = (await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).FirstOrDefault();
                    if (payment != null)
                    {
                        await reader.NextResultAsync();
                        GenericDataFactory<PaymentTransactionData> transactionDataFactory = new GenericDataFactory<PaymentTransactionData>();
                        payment.Transactions = (await transactionDataFactory.LoadData(reader, () => new PaymentTransactionData(), DataUtil.AssignDataStateManager)).ToList();
                    }
                });
            return payment;
        }

        public async Task<IEnumerable<PaymentData>> GetByLoanId(ISettings settings, Guid loanId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(ProviderFactory, "loanId", DbType.Guid, DataUtil.GetParameterValue(loanId))
            };
            List<PaymentData> payments = new List<PaymentData>();
            List<PaymentTransactionData> transactions = new List<PaymentTransactionData>();
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            await dataReaderProcess.Read(
                settings,
                ProviderFactory,
                "[ln].[GetPayment_by_LoanId]",
                CommandType.StoredProcedure,
                parameters,
                async reader =>
                {
                    payments = (await DataFactory.LoadData(reader, Create, DataUtil.AssignDataStateManager)).ToList();

                    await reader.NextResultAsync();
                    GenericDataFactory<PaymentTransactionData> transactionDataFactory = new GenericDataFactory<PaymentTransactionData>();
                    transactions = (await transactionDataFactory.LoadData(reader, () => new PaymentTransactionData(), DataUtil.AssignDataStateManager)).ToList();
                });
            return payments.GroupJoin(
                transactions,
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
