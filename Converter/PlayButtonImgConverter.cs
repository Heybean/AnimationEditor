using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace AnimationEditor.Converter
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class PlayButtonImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "pause.png" : "play.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "play.png" ? true : false;
        }
    }
}
