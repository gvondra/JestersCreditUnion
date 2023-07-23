namespace JestersCreditUnion.Testing.LoanGenerator
{
    public interface IDateGenerator
    {
        DateTime GenerateDate(int? dayRange, int? yearRange);
        DateTime GenerateDate(DateTime seed, int? dayRange, int? yearRange);
    }
}
