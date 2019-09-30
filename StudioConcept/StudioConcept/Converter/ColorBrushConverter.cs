using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DColor = System.Drawing.Color;

namespace StudioConcept.Converter
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
            {
                return new SolidColorBrush((Color)value);
            }
            else if (value is DColor)
            {
                
                return new SolidColorBrush(Color.FromArgb(((DColor)value).A, ((DColor)value).R, ((DColor)value).G, ((DColor)value).B));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
