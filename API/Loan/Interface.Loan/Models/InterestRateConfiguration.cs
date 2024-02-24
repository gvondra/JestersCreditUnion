namespace JestersCreditUnion.Interface.Loan.Models
{
    public class InterestRateConfiguration
    {
        public decimal? InflationRate { get; set; }
        public decimal? OperationsRate { get; set; }
        public decimal? LossRate { get; set; }
        public decimal? IncentiveRate { get; set; }
        public decimal? OtherRate { get; set; }
        public decimal? TotalRate { get; set; }
        public decimal? MinimumRate { get; set; }
        public decimal? MaximumRate { get; set; }
        public string OtherRateDescription { get; set; }
    }
}
