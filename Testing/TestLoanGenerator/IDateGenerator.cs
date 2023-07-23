namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface IDateGenerator
    {
        DateTime GenerateDate(int? dayRange = null, int? yearRange = null);
        DateTime GenerateDate(DateTime seed, int? dayRange = null, int? yearRange = null);
    }
}
