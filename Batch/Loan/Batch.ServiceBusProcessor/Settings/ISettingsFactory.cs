namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public interface ISettingsFactory
    {
        LoanSettings CreateLoan();
        ApiSettings CreateApi();
    }
}
