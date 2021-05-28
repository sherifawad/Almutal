using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Almutal.ValueConverters
{
    /// <summary>
    /// A converter that takes in a boolean and inverts it
    /// </summary>
    public class ListCountToHeightRequestConverter : BaseValueConverter<ListCountToHeightRequestConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (typeof(List<Strip>).IsAssignableFrom(value.GetType()))
            {
                return ((List<Strip>)value).Count * 30;
            }

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
