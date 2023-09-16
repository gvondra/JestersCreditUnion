namespace JCU.Internal
{
    public interface ISettingsFactory
    {
        JestersCreditUnion.Interface.ISettings CreateApi();
        JestersCreditUnion.Interface.ISettings CreateApi(string token);
        JestersCreditUnion.Interface.Loan.ISettings CreateLoanApi();
    }
}
