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
            double canvasSize = 0;
            double spriteSize = 0;
            double spriteScale = 0;

            if (values[0] != null)
                canvasSize = (double)values[0];
            if (values[1] is double)
                spriteSize = (double)values[1];
            if (values[2] is double)
                spriteScale = (double)values[2];

            return Math.Floor((canvasSize - spriteSize * spriteScale) / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
