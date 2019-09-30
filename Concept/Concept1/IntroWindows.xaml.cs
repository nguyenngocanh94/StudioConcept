using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Concept1
{
    /// <summary>
    /// Interaction logic for IntroWindows.xaml
    /// </summary>
    public partial class IntroWindows : Window
    {
        private readonly Storyboard show;
        private readonly Storyboard hide;

        private delegate void ShowDelegate(string txt);
        private delegate void HideDelegate();

        ShowDelegate _showDelegate;
        HideDelegate _hideDelegate;

        public IntroWindows()
        {
            InitializeComponent();
            _showDelegate = ShowText;
            _hideDelegate = HideText;

            show = Resources["ShowStoryBoard"] as Storyboard;
            hide = Resources["HideStoryBoard"] as Storyboard;

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await this.Load();
        }

        private async Task Load()
        {
            await Task.Delay(100);
            Dispatcher.Invoke(() => { ProgressBar.Value = 20; });
            Dispatcher.Invoke(_showDelegate, Properties.Resources.loading_mainwindow);
            await Task.Delay(500);
            Dispatcher.Invoke(() => { ProgressBar.Value = 40; });
            //load data 
            Dispatcher.Invoke(_hideDelegate);
            await Task.Delay(100);
            Dispatcher.Invoke(() => { ProgressBar.Value = 50; });
            Dispatcher.Invoke(_showDelegate, Properties.Resources.loading_uia_instance);
            await Task.Delay(500);
            Dispatcher.Invoke(() => { ProgressBar.Value = 60; });
            //load data
            Dispatcher.Invoke(_hideDelegate);
            Dispatcher.Invoke(() => { ProgressBar.Value = 70; });
            await Task.Delay(100);
            Thread.Sleep(100);
            Dispatcher.Invoke(_showDelegate, Properties.Resources.loading_web_automatinon_instance);
            Dispatcher.Invoke(() => { ProgressBar.Value = 80; });
            await Task.Delay(500);
            //load data 
            Dispatcher.Invoke(_hideDelegate);
            Dispatcher.Invoke(() => { ProgressBar.Value = 100; });

            //close the window
            await Task.Delay(200);
            Dispatcher.Invoke(() => this.Close());
        }
        private void ShowText(string txt)
        {
            StatusText.Text = txt;
            BeginStoryboard(show);
        }
        private void HideText()
        {
            BeginStoryboard(hide);
        }
    }
}
