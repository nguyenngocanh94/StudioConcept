using Concept1.CustomControl;
using Concept1.Models;
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
using System.Windows.Shapes;
using Concept1.DragDropFramework;
using Concept1.DragDropFrameworkData;
using GongSolutions.Wpf.DragDrop;

namespace Concept1
{
    /// <summary>
    /// Interaction logic for BtcRpa1.xaml
    /// </summary>
    public partial class BtcRpa1 : Window
    {
        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(BtcRpa1), new PropertyMetadata(new OnlyCopyDropHandler()));

        public ObservableCollection<RpaAction> ActionsList { get; set; } = new ObservableCollection<RpaAction>();
        public ObservableCollection<RpaAction> ScenarioActionsList { get; set; } = new ObservableCollection<RpaAction>();

        public BtcRpa1()
        {
            InitializeComponent();

            new IntroWindows().ShowDialog();

            var favorite = new RpaAction() { Name = Properties.Resources.favorite };
            this.ActionsList.Add(favorite);

            var recently = new RpaAction() { Name = Properties.Resources.recent_actions };
            this.ActionsList.Add(recently);

            var webBrowser = new RpaAction() { Name = Properties.Resources.web_browser_action };
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_google_search, Icon = "/Images/google.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_yahoo_japan_search, Icon = "/Images/yahoo.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_yahoo_japan_finance, Icon = "/Images/yahoo.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_yahoo_japan_transit, Icon = "/Images/yahoo.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_wikipedia_search, Icon = "/Images/wiki.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_open_browser, Icon = "/Images/browser.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_close_browser, Icon = "/Images/close.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_minimize_browser, Icon = "/Images/minus.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_maximize_browser, Icon = "/Images/expand.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_switch_browser, Icon = "/Images/command.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_goto_url, Icon = "/Images/url.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_data_extraction, Icon = "/Images/data.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_enter_data, Icon = "/Images/tag.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_back, Icon = "/Images/left.png" });
            webBrowser.Children.Add(new RpaAction() { Name = Properties.Resources.action_next, Icon = "/Images/right.png" });
            this.ActionsList.Add(webBrowser);
            var operation = new RpaAction() { Name = Properties.Resources.operation_action };
            this.ActionsList.Add(operation);

            var applications = new RpaAction() { Name = Properties.Resources.application_action };
            this.ActionsList.Add(applications);

            var mails = new RpaAction() { Name = Properties.Resources.mail_action };
            this.ActionsList.Add(mails);

            var sns = new RpaAction() { Name = Properties.Resources.sns_action };
            this.ActionsList.Add(sns);

            this.ScenarioActionsList.Add(new RpaAction() { Name = Properties.Resources.action_google_search, Icon = "/Images/google.png" });
            this.ScenarioActionsList.Add(new RpaAction() { Name = Properties.Resources.action_minimize_browser, Icon = "/Images/minus.png", Description="minimize the window and wain't a munites"});
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SeeDetail(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource.GetType().ToString();
            if (source == "System.Windows.Controls.Grid" || source== "System.Windows.Shapes.Path")
            {
                return;
            }
            
            var action = (RpaAction)(sender as Control).DataContext;
            
            if (MainGrid.ColumnDefinitions.Count == 3)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(320)
                });
                var property = new MailDetail();
                
                Grid.SetRow(property, 2);
                Grid.SetColumn(property, 4);
                MainGrid.Children.Add(property);
            }
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ScenarioSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = this.ScenarioSearch.Text?.ToLower();

            foreach (var action in this.ScenarioActionsList)
            {
                if (string.IsNullOrWhiteSpace(text)
                    || (action.Name.ToLower().Contains(text)))
                {
                    action.Visibility = Visibility.Visible;
                }
                else
                {
                    action.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ActionSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = this.ActionSearch.Text?.ToLower();

            foreach (var item in this.ActionsList)
            {
                foreach (var action in item.Children)
                {
                    if (string.IsNullOrWhiteSpace(text)
                        || (action.Name.ToLower().Contains(text)))
                    {
                        action.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        action.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void ActionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = this.ActionListBox.SelectedItem;

            if ((selectedItem == null)
                && (this.MainGrid.ColumnDefinitions.Count == 4))
            {
                MailDetail property = null;

                foreach (var child in this.MainGrid.Children)
                {
                    if (child is MailDetail)
                    {
                        property = child as MailDetail;
                        break;
                    }
                }

                if (property != null)
                {
                    this.MainGrid.Children.Remove(property);
                }

                this.MainGrid.ColumnDefinitions.RemoveAt(3);
            }
            else if ((selectedItem != null)
                && (this.MainGrid.ColumnDefinitions.Count == 3))
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(320)
                });
                var property = new MailDetail();

                Grid.SetRow(property, 2);
                Grid.SetColumn(property, 4);
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

        public static object GetObjectAtPoint<ItemContainer>(ItemsControl control, Point p)
        where ItemContainer : DependencyObject
        {
            // ItemContainer - can be ListViewItem, or TreeViewItem and so on(depends on control)
            ItemContainer obj = GetContainerAtPoint<ItemContainer>(control, p);
            if (obj == null)
                return null;

            return control.ItemContainerGenerator.ItemFromContainer(obj);
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

        private void DeleteScenarioActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var action = button.Tag as RpaAction;
            this.ScenarioActionsList.Remove(action);
        }
        //        private Point startPoint;
        //
        //        private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //        {
        //            startPoint = e.GetPosition(null);
        //        }
        //
        //        private void List_MouseMove(object sender, MouseEventArgs e)
        //        {
        //            Point mousePos = e.GetPosition(null);
        //            Vector diff = startPoint - mousePos;
        //
        //            if (e.LeftButton == MouseButtonState.Pressed &&
        //                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
        //            {
        //                // Get the dragged ListViewItem
        //                TreeView treeView = sender as TreeView;
        //                TreeViewItem treeViewItem =
        //                    FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource);
        //
        //                // Find the data behind the ListViewItem
        //                RpaAction rpaAction = (RpaAction)treeView.ItemContainerGenerator.
        //                    ItemFromContainer(treeViewItem);
        //
        //                 Initialize the drag & drop operation
        //                DataObject dragData = new DataObject("myFormat", contact);
        //                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
        //            }
        //        }

        //        private static T FindAnchestor<T>(DependencyObject current)
        //            where T : DependencyObject
        //        {
        //            do
        //            {
        //                if (current is T)
        //                {
        //                    return (T)current;
        //                }
        //                current = VisualTreeHelper.GetParent(current);
        //            }
        //            while (current != null);
        //            return null;
        //        }
    }
}
 