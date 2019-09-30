using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MUIConcept.CustomControl
{
    /// <summary>
    /// Interaction logic for ActionProperty.xaml
    /// </summary>
    public partial class ActionProperty : UserControl
    {
        public ActionProperty()
        {
            InitializeComponent();
            SelectFile = new SimpleCommand((textbox)=> {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    (textbox as TextBox).Text =  openFileDialog.FileName;
            });
            DataContext = this;
        }

        public ICommand SelectFile { get; set; }
    }

    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<Control> commandAction;

        public SimpleCommand(Action<Control> commandAction)
        {
            this.commandAction = commandAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.commandAction(parameter as Control);
        }
    }
}
