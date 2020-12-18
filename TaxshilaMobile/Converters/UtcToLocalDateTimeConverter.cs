using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TaxshilaMobile.Converters
{
    public class UtcToLocalDateTimeConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return "";
            if (value is DateTime)
            {
                DateTime dt = default;
                if ((DateTime)value == dt) return "";
                return ((DateTime)value).ToLocalTime();
            }

            else
                return DateTime.Parse(value?.ToString()).ToLocalTime();
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
                return ((DateTime)value).ToUniversalTime();
            else
            {
                DateTime.TryParse(value?.ToString(), out DateTime dt);

                return dt.ToUniversalTime();
            }


        }
    }
}
