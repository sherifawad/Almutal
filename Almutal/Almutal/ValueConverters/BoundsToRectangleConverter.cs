using DataBase.Models;
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
    public class BoundsToRectangleConverter : BaseValueConverter<BoundsToRectangleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Box box)
                return new Rectangle(box.Position.X * 1.2, box.Position.Y * 1.2, box.Width * 1.2, box.Length * 1.2);
            else
                return new Rectangle(0, 0, 1, 1);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
