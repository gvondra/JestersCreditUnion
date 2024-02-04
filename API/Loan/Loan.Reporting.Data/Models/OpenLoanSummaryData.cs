namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class OpenLoanSummaryData
    {
        [ColumnMapping] public string Number { get; set; }
        [ColumnMapping] public decimal Balance { get; set; }
        [ColumnMapping] public DateTime NextPaymentDue { get; set; }
    }
}
