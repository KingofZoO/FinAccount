using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinAccount.Converters {
    public class BoolToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value as bool?).Value == true ? Color.Green : Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
