using System;
using System.Collections.Generic;
using System.Globalization;

namespace JestersCreditUnion.Loan.Core
{
    public sealed class LoanNumberGenerator
    {
        private static readonly SortedSet<string> _generated = new SortedSet<string>();

        public string Generate()
        {
            string number = string.Empty;
            bool foundNumber = false;
            while (!foundNumber)
            {
                string year = DateTime.Today.ToString("yy", CultureInfo.InvariantCulture.DateTimeFormat);
                DateTime rootDate = new DateTime(DateTime.Today.AddYears(-2).Year, 1, 1);
                double milliseconds = Math.Floor(DateTime.Now.Subtract(rootDate).TotalMilliseconds);
                milliseconds = milliseconds * Math.Pow(10, -7);
                milliseconds = (milliseconds - Math.Floor(milliseconds)) * Math.Pow(10, 7);
                number = year + Convert.ToInt64(Math.Floor(milliseconds)).ToString("0000000", CultureInfo.InvariantCulture.NumberFormat);
                lock (_generated)
                {
                    foundNumber = _generated.Add(number);
                }
            }
            return number;
        }
    }
}
