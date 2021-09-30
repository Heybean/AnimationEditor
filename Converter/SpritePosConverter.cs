using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AnimationEditor.Converter
{
    class SpritePosConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double canvasSize = (double)values[0];
            int origin = (int)values[1];
            double scale = (double)values[2];

            return (double)Math.Floor((int)(canvasSize / 2) - origin * scale);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
