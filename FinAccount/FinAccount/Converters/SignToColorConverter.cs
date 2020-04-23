using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinAccount.Converters {
    public class SignToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value as decimal?).Value >= 0 ? Color.Green : Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
