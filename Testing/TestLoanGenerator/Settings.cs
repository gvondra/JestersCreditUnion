namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class Settings
    {
        public string ApiBaseAddress { get; set; }
        public string LoanApiBaseAddress { get; set; }
        public string LoanApplicationFile { get; set; }
        public int LoanApplicationCount { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
