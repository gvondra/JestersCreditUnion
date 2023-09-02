namespace JestersCreditUnion.Interface.Models
{
    public class LoanPaymentAmountRequest
    {
        public decimal? TotalPrincipal { get; set; }
        public decimal? AnnualInterestRate { get; set; }
        public short? Term { get; set; }
        public short PaymentFrequency { get; set; } = 12;
    }
}
