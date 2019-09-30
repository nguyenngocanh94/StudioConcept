using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StudioConcept.MVVM;

namespace StudioConcept.Command
{
    public class BaseCommand : ICommand
    {
        private Action<MouseEventArgs> _commandAction;

        public BaseCommand(Action<MouseEventArgs> commandAction)
        {
            _commandAction = commandAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _commandAction((MouseEventArgs)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
