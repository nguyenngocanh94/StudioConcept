using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using StudioConcept.MVVM;

namespace StudioConcept.Converter
{
    public class ChildrenPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var shape = values[0];
            if (shape is IBranch)
            {
                if (shape is IfShape)
                {
                    double total = 8;
                    if ((shape as IfShape).ChildrenNode.Count > 0)
                    {
                        // todo trigger add then calculate middle height.
                        ((IfShape)shape).ChildrenNode[1].InnerY = 38;
                        //((IfShape)shape).ChildrenNode.ForEach(sh => { total += ((Sequence)sh).Height; });
                        ((IfShape)shape).MiddleSpace = total;
                        return Calculate((BaseShape)shape, (string)parameter == "Canvas.Left");
                    }
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double Calculate(BaseShape parent, bool isLeft)
        {
            if (isLeft)
            {
                if (parent is IfShape)
                {
                    //((IfShape) parent).MiddleSpace = 40;
                    return 16;
                }
            }
            else
            {
                if (parent is IfShape)
                {
                    return 20 + 4 * 2;
                }
            }

            return 0;
        }
    }
}
