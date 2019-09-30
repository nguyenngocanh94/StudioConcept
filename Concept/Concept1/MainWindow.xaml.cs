using System.Collections.Generic;
using System.Windows;
using Concept1.Models;

namespace Concept1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Project> items = new List<Project>();
            items.Add(new Project{Name = "Bp", Path = "D:/Rpa/Bp/customer1/..."});
            items.Add(new Project{Name = "UiPath", Path = "D:/Rpa/UiPath/customer2/..." });
            items.Add(new Project{Name = "AA", Path = "D:/Rpa/AA/customerN/..." });
            RecentProject.ItemsSource = items;
            new IntroWindows().ShowDialog();
        }
    }
}
