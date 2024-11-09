using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace JCU.Internal.Converter
{
    public class DecimalToStringConverter : IValueConverter
    {
        public decimal? FallbackValue { get; set; } = 0.0M;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.ToString() ?? string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = DependencyProperty.UnsetValue;
            if (value is string stringValue)
            {
                stringValue = stringValue?.Trim() ?? string.Empty;
                if (stringValue == string.Empty && targetType.Equals(typeof(decimal?)))
                    result = FallbackValue;
                else if (Regex.IsMatch(stringValue, @"^-?(?:[0-9]+\.?[0-9]*)|(?:[0-9]*\.?[0-9]+)$"))
                    result = decimal.Parse(stringValue);
            }
            return result;
        }
    }
}
