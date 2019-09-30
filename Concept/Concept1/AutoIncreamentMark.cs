using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Concept1
{
    public class AutoIncreamentMark : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var listboxItem = value as ListBoxItem;
            var listBox = FindAncestor<ListBox>(listboxItem);
            if (listBox != null)
            {
                var index = listBox.Items.IndexOf(listboxItem.Content);
                return index+1;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = int.Parse(value.ToString());
            return number - 1;
        }

        public static T FindAncestor<T>(DependencyObject from) where T : class
        {
            if (from == null)
                return null;

            var candidate = from as T;
            return candidate ?? FindAncestor<T>(VisualTreeHelper.GetParent(from));
        }
    }
}
