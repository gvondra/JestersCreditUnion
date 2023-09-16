namespace JestersCreditUnion.Interface.Loan.Models
{
    public class DisburseResponse
    {
        public Loan Loan { get; set; }
        public Transaction Transaction { get; set; }
    }
}
