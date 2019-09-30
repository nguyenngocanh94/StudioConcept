using Concept1.CustomControl;
using Concept1.Models;
using System.Windows;
using System.Windows.Controls;

namespace Concept1
{
    public class ActionTemplateSwitcher : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            RpaAction action = item as RpaAction;
            var temp = new ActionBar();
            return base.SelectTemplate(item, container);
        }
    }
}
