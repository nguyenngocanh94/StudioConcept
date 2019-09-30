using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SearchWithMagGlass.xaml
    /// </summary>
    public partial class SearchWithMagGlass : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchWithMagGlass));

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public SearchWithMagGlass()
        {
            InitializeComponent();
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = this.SearchText.Text;
            this.TextChanged?.Invoke(sender, e);
        }
    }
}
