namespace JestersCreditUnion.Interface.Loan.Models
{
    public class AmortizationItem
    {
        public short? Term { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
    }
}
