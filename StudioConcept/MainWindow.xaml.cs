using System.Windows;
using StudioConcept.MVVM;
using Unity;

namespace StudioConcept
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IUnityContainer container = new UnityContainer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowMVVM(this);
        }
    }
}
