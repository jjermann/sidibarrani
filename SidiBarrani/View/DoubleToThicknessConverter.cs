using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace SidiBarrani.View {
    public class DoubleToThicknessConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var thickness = value is double
                ? new Thickness((double)value)
                : new Thickness(0);
            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Thickness && ((Thickness)value).IsUniform)
            {
                return ((Thickness)value).Left;
            }
            throw new NotImplementedException();
        }
    }
}