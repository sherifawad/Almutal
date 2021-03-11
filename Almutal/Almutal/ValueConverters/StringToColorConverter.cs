using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Almutal.ValueConverters
{
    /// <summary>
    /// A converter that takes in a boolean and inverts it
    /// </summary>
    public class StringToColorConverter : BaseValueConverter<StringToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string && (string)value != "")
                return Color.FromHex(value.ToString());
            else
                return Color.Transparent;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
