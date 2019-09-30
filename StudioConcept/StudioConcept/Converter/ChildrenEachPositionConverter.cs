using System;
using System.Globalization;
using System.Windows.Data;
using StudioConcept.MVVM;

namespace StudioConcept.Converter
{
    public class ChildrenEachPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var shape = values[0];
            if (shape is IfShape)
            {
                return 0;
            }

            return 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
