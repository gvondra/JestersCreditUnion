using System;
using System.Text.RegularExpressions;

namespace JCU.Internal
{
    public static class StringFormatter
    {
        public static string PhoneNumber(string number)
        {
            number = (number ?? string.Empty).Trim();
            if (number.Length > 0 )
            {
                number = Regex.Replace(number, @"[^0-9]+", string.Empty, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));
                if (Regex.IsMatch(number, @"^[0-9]{10}$")) 
                    number = string.Format("{0}-{1}-{2}", number.Substring(0, 3), number.Substring(3, 3), number.Substring(6));
                else if (Regex.IsMatch(number, @"^[0-9]{7}$"))
                    number = string.Format("{0}-{1}", number.Substring(0, 3), number.Substring(3));
            }
            return number;
        }
    }
}
