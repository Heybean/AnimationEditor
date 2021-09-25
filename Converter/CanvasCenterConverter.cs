using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace AnimationEditor.Converter
{
    // Converter to centerize the objects in rendering view based on its size and render scale. This should be a one way conversion.
    public class CanvasCenterConverter : IMultiValueConverter
    {
        // values: [0] = canvas size (width or height), [1] = sprite width or height, [2] = render scale (x or y)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Floor(((double)values[0] - (float)values[1] * (double)values[2]) / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
