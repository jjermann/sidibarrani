using System;
using System.Globalization;
using System.Windows;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace SidiBarrani.View {
    public class StringToBitmapConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var imageSource = value as string;
            if (imageSource == null)
            {
                return null;
            }
            var bitmap = new Bitmap(imageSource);
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}