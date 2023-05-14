using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Core
{
    public sealed class LoanNumberGenerator
    {
        private static SortedSet<string> _generated = new SortedSet<string>();

        public string Generate()
        {
            string number = string.Empty;
            bool foundNumber = false;
            while (!foundNumber)
            {
                string year = DateTime.Today.ToString("yy");
                DateTime rootDate = new DateTime(DateTime.Today.AddYears(-2).Year, 1, 1);
                double milliseconds = Math.Floor(DateTime.Now.Subtract(rootDate).TotalMilliseconds);
                milliseconds = milliseconds * Math.Pow(10, -7);
                milliseconds = (milliseconds - Math.Floor(milliseconds)) * Math.Pow(10, 7);
                number = year + Convert.ToInt64(Math.Floor(milliseconds)).ToString("0000000");
                lock (_generated)
                {
                    foundNumber = _generated.Add(number);
                }
            }
            return number;
        }
    }
}
