namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanBalanceData
    {
        [ColumnMapping("Id")] public long Id { get; set; }
        [ColumnMapping("Date")] public DateTime Date { get; set; }
        [ColumnMapping("Balance")] public decimal Balance { get; set; }
        [ColumnMapping("DaysPastDue")] public short? DaysPastDue { get; set; }
        [ColumnMapping("LoanId")] public long LoanId { get; set; }
        [ColumnMapping("LoanAgreementId")] public long LoanAgreementId { get; set; }
        [ColumnMapping("LoanStatus")] public short LoanStatusId { get; set; }
        public LoanStatusData LoanStatus { get; set; }
        public LoanData Loan { get; set; }
        public LoanAgreementData LoanAgreement { get; set; }
    }
}
