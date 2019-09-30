using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using StudioConcept.MVVM;

namespace StudioConcept.Converter
{
    public class ShapeTextPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) parameter=="Canvas.Left")
            {
                return Calculate((BaseShape) value, true);
            }
            if ((string)parameter == "Canvas.Top")
            {
                return Calculate((BaseShape)value, false);
            }

            return CalculateTemplate((BaseShape) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double Calculate(BaseShape shape, bool isLeft)
        {
            if (shape is Sequence)
            {
                if (isLeft)
                {
                    return ((Sequence)shape).Width / 10;
                }

                return (((Sequence)shape).Height - ((Sequence)shape).FontSize) / 2;
            }

            return 1;
        }

        private Thickness CalculateTemplate(BaseShape shape)
        {
            if (shape is Sequence)
            {
                return new Thickness(10, (((Sequence)shape).Height - ((Sequence)shape).FontSize) / 2,0,0);
            }

            return new Thickness(0);
        }
    }
}
