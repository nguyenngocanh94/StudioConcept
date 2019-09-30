using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Concept1
{
    /// <summary>
    /// SelectCulture.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectCulture : Window
    {
        public ObservableCollection<LanguageItem> Languages { get; set; } = new ObservableCollection<LanguageItem>();

        public LanguageItem SelectedLanguage
        {
            get { return (LanguageItem)GetValue(SelectedLanguageProperty); }
            set { SetValue(SelectedLanguageProperty, value); }
        }

        public static readonly DependencyProperty SelectedLanguageProperty =
            DependencyProperty.Register("SelectedLanguage", typeof(LanguageItem), typeof(SelectCulture));

        public SelectCulture()
        {
            InitializeComponent();

            this.Languages.Add(new LanguageItem()
            {
                CultureInfo = CultureInfo.GetCultureInfo("ja"),
                DisplayName = "日本語",
            });

            this.Languages.Add(new LanguageItem()
            {
                CultureInfo = CultureInfo.GetCultureInfo("en"),
                DisplayName = "English",
            });

            this.SelectedLanguage = this.Languages[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = this.SelectedLanguage.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = this.SelectedLanguage.CultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = this.SelectedLanguage.CultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = this.SelectedLanguage.CultureInfo;
            this.Close();
        }
    }

    public class LanguageItem : DependencyObject
    {
        public CultureInfo CultureInfo
        {
            get { return (CultureInfo)GetValue(CultureInfoProperty); }
            set { SetValue(CultureInfoProperty, value); }
        }

        public static readonly DependencyProperty CultureInfoProperty =
            DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(LanguageItem), new PropertyMetadata(CultureInfo.CurrentCulture));

        
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(LanguageItem));
    }
}
