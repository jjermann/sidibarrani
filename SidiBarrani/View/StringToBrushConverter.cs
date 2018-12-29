using System;
using System.Globalization;
using System.Windows;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace SidiBarrani.View {
    public class StringToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var brush = !string.IsNullOrEmpty(value as string)
                ? Brush.Parse((string)value)
                : (Brush)null;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var brushStr = value is Brush
                ? ((Brush)value).ToString()
                : (string)null;
            return brushStr;
        }
    }
}