namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class DateGenerator : IDateGenerator
    {
        private static readonly Random _random = new Random();

        public DateTime GenerateDate(int? dayRange, int? yearRange)
            => GenerateDate(DateTime.Today, dayRange, yearRange);

        public DateTime GenerateDate(DateTime seed, int? dayRange, int? yearRange)
        {
            DateTime result = default(DateTime);
            if (dayRange.HasValue && yearRange.HasValue)
            {
                throw new ArgumentException($"Only one of {nameof(dayRange)} or {nameof(yearRange)} can be specified");
            }   
            else if (!dayRange.HasValue && !yearRange.HasValue)
            {
                throw new ArgumentException($"One of {nameof(dayRange)} or {nameof(yearRange)} must be specified");
            }   
            else if (dayRange.HasValue)
            {
                int dayOffset = _random.Next(dayRange.Value) * -1;
                result = seed.AddDays(dayOffset);
            }
            else if (yearRange.HasValue)
            {
                 int yearOffset = _random.Next(yearRange.Value) * -1;
                result = seed.AddYears(yearOffset);
            }
            return result;
        }
    }
}
