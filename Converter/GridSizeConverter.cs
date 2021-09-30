using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AnimationEditor.Converter
{
    [ValueConversion(typeof(double), typeof(GridLength))]
    public class GridSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = (GridLength)value;
            return obj.Value;
        }
    }
}
