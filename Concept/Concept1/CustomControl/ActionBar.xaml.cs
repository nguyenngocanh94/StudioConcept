using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concept1.CustomControl
{
    /// <summary>
    /// Interaction logic for ActionBar.xaml
    /// </summary>
    public partial class ActionBar : UserControl
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(ActionBar));
        public static readonly DependencyProperty ActionNameProperty = DependencyProperty.Register("ActionName", typeof(string), typeof(ActionBar));
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(string), typeof(ActionBar));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ActionBar));

        public ActionBar()
        {
            InitializeComponent();
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public string ActionName
        {
            get { return (string)GetValue(ActionNameProperty); }
            set
            {
                SetValue(ActionNameProperty, value);
            }
        }

        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set
            {
                SetValue(NumberProperty, value);
            }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }
    }
}
