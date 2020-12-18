using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TaxshilaMobile.Converters
{
    public class BoolInversorConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return false;
            }
            else if (value is bool)
            {
                return !(bool)value;
            }
            else
            {
                throw new InvalidOperationException("The converter only accepts boolean values");
            }
        }
    }
}
