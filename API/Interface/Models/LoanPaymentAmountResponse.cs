namespace JestersCreditUnion.Interface.Models
{
    public class LoanPaymentAmountResponse : LoanPaymentAmountRequest
    {
        public decimal? PaymentAmount { get; set; }
    }
}
