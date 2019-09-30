using System.Windows;
using System.Windows.Controls;


namespace Concept1.CustomControl
{
    /// <summary>
    /// Interaction logic for MainFunction.xaml
    /// </summary>
    public partial class MainFunction : UserControl
    {
        public static readonly DependencyProperty TheContentProperty = DependencyProperty.Register("TheContent", typeof(string), typeof(MainFunction));
        public static readonly DependencyProperty TheImageProperty = DependencyProperty.Register("TheImage", typeof(string), typeof(MainFunction));


        public string TheContent {
            get { return (string)GetValue(TheContentProperty); }
            set {
              SetValue(TheContentProperty, value);
            }
        }
        public string TheImage {
            get { return (string)GetValue(TheImageProperty); }
            set { SetValue(TheImageProperty, value); }
        }

        public MainFunction()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
