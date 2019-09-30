using System;
using System.Windows;
using System.Windows.Controls;


namespace MUIConcept.CustomControl
{
    /// <summary>
    /// Interaction logic for ActionButtonControl.xaml
    /// </summary>
    public partial class ActionButtonControl : UserControl
    {
        public static readonly DependencyProperty ActionNameProp = DependencyProperty.Register("ActionName", typeof(string), typeof(ActionButtonControl));
        public static readonly DependencyProperty ImagePathProp = DependencyProperty.Register("ImagePath", typeof(string), typeof(ActionButtonControl));
        public ActionButtonControl()
        {
            InitializeComponent();
        }

        public string ActionName
        {
            get { return (string)GetValue(ActionNameProp); }
            set
            {
                SetValue(ActionNameProp, value);
            }
        }

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProp); }
            set
            {
                value = "../Image/" + value;
                SetValue(ImagePathProp, value);
            }
        }
    }
}
