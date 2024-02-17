namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanData
    {
        [ColumnMapping("LoanId", IsPrimaryKey = true)] public long LoanId { get; set; }
        [ColumnMapping("Number")] public string Number { get; set; }
        [ColumnMapping("InitialDisbursementDate")] public DateTime? InitialDisbursementDate { get; set; }
        [ColumnMapping("FirstPaymentDue")] public DateTime? FirstPaymentDue { get; set; }
        [ColumnMapping("NextPaymentDue")] public DateTime? NextPaymentDue { get; set; }
    }
}
