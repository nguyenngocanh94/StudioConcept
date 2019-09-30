using System.Windows;
using System.Windows.Controls;


namespace Concept1.CustomControl
{
    /// <summary>
    /// Interaction logic for MainFunctionWithButton.xaml
    /// </summary>
    public partial class MainFunctionWithButton : UserControl
    {
        public static readonly DependencyProperty TheContentProperty = DependencyProperty.Register("TheContentWithButton", typeof(string), typeof(MainFunctionWithButton));
        public static readonly DependencyProperty TheImageProperty = DependencyProperty.Register("TheImageWithButton", typeof(string), typeof(MainFunctionWithButton));
        public static readonly DependencyProperty TheFontSizeProperty = DependencyProperty.Register("TheFontsize", typeof(string), typeof(MainFunctionWithButton));

        public string TheContentWithButton
        {
            get { return (string)GetValue(TheContentProperty); }
            set
            {
                SetValue(TheContentProperty, value);
            }
        }
        public string TheImageWithButton
        {
            get { return (string)GetValue(TheImageProperty); }
            set { SetValue(TheImageProperty, value); }
        }

        public string TheFontsize
        {
            get { return (string)GetValue(TheFontSizeProperty); }
            set { SetValue(TheFontSizeProperty, value); }
        }

        public MainFunctionWithButton()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
