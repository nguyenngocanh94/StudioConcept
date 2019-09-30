using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Concept1.Models;
using MUIConcept.CustomControl;

namespace MUIConcept
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public ObservableCollection<RpaAction> ScenarioActionsList { get; set; } = new ObservableCollection<RpaAction>();
        public List<string> Zooms;
        public MainWindow()
        {
            InitializeComponent();
            Zooms = new List<string>();
            Zooms.Add("10%");
            Zooms.Add("20%");
            Zooms.Add("30%");
            Zooms.Add("40%");
            Zooms.Add("50%");
            Zooms.Add("60%");
            Zooms.Add("70%");
            Zooms.Add("80%");
            Zooms.Add("90%");
            Zooms.Add("100%");
            ScenarioActionsList.Add(new RpaAction() { Name = "Google search", Icon = "/Image/google.png" });
            ScenarioActionsList.Add(new RpaAction() { Name = "Minium Browser", Icon = "/Image/minus.png", Description = "minimize the window and wain't a munites" });
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ActionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = this.ActionSearch.Text?.ToLower();

            foreach (var item in this.ScenarioActionsList)
            {
                if (string.IsNullOrWhiteSpace(text)
                        || (item.Name.ToLower().Contains(text)))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void DeleteScenarioActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var action = button.Tag as RpaAction;
            this.ScenarioActionsList.Remove(action);
        }

       

        private void RemoveAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.ScenarioActionsList)
            {
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                }
            }

            CheckAll.IsChecked = false;
        }

        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.ScenarioActionsList)
            {
                if (!item.IsSelected)
                {
                    item.IsSelected = true;
                }
            }
        }

        private void ActionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = this.ActionListBox.SelectedItem;
            if(selectedItem == null && MainGrid.ColumnDefinitions.Count == 2)
            {
                ActionProperty property = null;

                foreach (var child in MainGrid.Children)
                {
                    if (child is ActionProperty)
                    {
                        property = child as ActionProperty;
                        break;
                    }
                }

                if (property != null)
                {
                    MainGrid.Children.Remove(property);
                }

                MainGrid.ColumnDefinitions.RemoveAt(1);
            }
            else if ((selectedItem != null)
               && (MainGrid.ColumnDefinitions.Count == 1))
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(390)
                });
                var property = new ActionProperty();
                property.Margin = new Thickness { Left = 10, Bottom = 0, Top = 0, Right = 0 };
                Grid.SetRow(property, 1);
                Grid.SetColumn(property, 1);
                MainGrid.Children.Add(property);
            }
        }

        private void ActionListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var container = GetContainerAtPoint<ListBoxItem>(this.ActionListBox, e.GetPosition(this.ActionListBox));

            if (container == null)
            {
                this.ActionListBox.SelectedItem = null;
            }
        }

        public static ItemContainer GetContainerAtPoint<ItemContainer>(ItemsControl control, Point p)
       where ItemContainer : DependencyObject
        {
            HitTestResult result = VisualTreeHelper.HitTest(control, p);
            DependencyObject obj = result.VisualHit;

            while (VisualTreeHelper.GetParent(obj) != null && !(obj is ItemContainer))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            // Will return null if not found
            return obj as ItemContainer;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
