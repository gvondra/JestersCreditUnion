namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanAgreementData
    {
        [ColumnMapping("LoanAgreementId", IsPrimaryKey = true)] public long LoanAgreementId { get; set; }
        [ColumnMapping("Hash")] public byte[] Hash { get; set; }
        [ColumnMapping("CreateDate")] public DateTime CreateDate { get; set; }
        [ColumnMapping("AgreementDate")] public DateTime? AgreementDate { get; set; }
        [ColumnMapping("InterestRate")] public decimal InterestRate { get; set; }
        [ColumnMapping("PaymentAmount")] public decimal PaymentAmount { get; set; }
    }
}
