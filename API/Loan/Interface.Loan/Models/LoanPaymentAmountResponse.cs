namespace JestersCreditUnion.Interface.Loan.Models
{
    public class LoanPaymentAmountResponse : LoanPaymentAmountRequest
    {
        public decimal? PaymentAmount { get; set; }
    }
}
