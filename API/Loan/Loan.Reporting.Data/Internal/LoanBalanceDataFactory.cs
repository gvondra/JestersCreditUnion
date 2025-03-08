using JestersCreditUnion.Loan.Reporting.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Reporting.Data.Internal
{
    public class LoanBalanceDataFactory : ILoanBalanceDataFactory
    {
        private readonly IDbProviderFactory providerFactory;

        public LoanBalanceDataFactory(IDbProviderFactory providerFactory)
        {
            this.providerFactory = providerFactory;
        }

        public async Task<IEnumerable<LoanBalanceData>> GetAll(ISettings settings)
        {
            List<LoanBalanceData> result = new List<LoanBalanceData>();
            List<LoanStatusData> statuses = new List<LoanStatusData>();
            List<LoanData> loans = new List<LoanData>();
            List<LoanAgreementData> agreements = new List<LoanAgreementData>();
            DataReaderProcess dataReaderProcess = new DataReaderProcess();
            await dataReaderProcess.Read(
                settings,
                providerFactory,
                "[lnrpt].[GetLoanBalances]",
                commandType: CommandType.StoredProcedure,
                readAction: async reader =>
                {
                    GenericDataFactory<LoanStatusData> statusFactory = new GenericDataFactory<LoanStatusData>();
                    statuses = (await statusFactory.LoadData(reader, () => new LoanStatusData()))
                        .ToList();

                    await reader.NextResultAsync();
                    GenericDataFactory<LoanData> loanFactory = new GenericDataFactory<LoanData>();
                    loans = (await loanFactory.LoadData(reader, () => new LoanData()))
                        .ToList();

                    await reader.NextResultAsync();
                    GenericDataFactory<LoanAgreementData> agreementFactory = new GenericDataFactory<LoanAgreementData>();
                    agreements = (await agreementFactory.LoadData(reader, () => new LoanAgreementData()))
                        .ToList();

                    await reader.NextResultAsync();
                    GenericDataFactory<LoanBalanceData> loanBalanceFactory = new GenericDataFactory<LoanBalanceData>();
                    result = (await loanBalanceFactory.LoadData(reader, () => new LoanBalanceData()))
                        .ToList();
                });

            result = result.Join(
                statuses,
                bal => bal.LoanStatusId,
                stts => stts.Status,
                (LoanBalanceData bal, LoanStatusData stts) =>
                {
                    bal.LoanStatus = stts;
                    return bal;
                })
                .GroupJoin(
                loans,
                bal => bal.LoanId,
                ln => ln.LoanId,
                (LoanBalanceData bal, IEnumerable<LoanData> ln) =>
                {
                    bal.Loan = ln.FirstOrDefault();
                    return bal;
                })
                .GroupJoin(
                agreements,
                bal => bal.LoanAgreementId,
                agmt => agmt.LoanAgreementId,
                (LoanBalanceData bal, IEnumerable<LoanAgreementData> agmt) =>
                {
                    bal.LoanAgreement = agmt.FirstOrDefault();
                    return bal;
                })
                .ToList();

            return result;
        }
    }
}
