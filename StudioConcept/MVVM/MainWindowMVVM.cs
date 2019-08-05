using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StudioConcept.MVVM
{
    class MainWindowMVVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<BaseShape> shapes = new ObservableCollection<BaseShape>();

        public ObservableCollection<BaseShape> Shapes
        {
            get
            {
                return shapes;
            }
        }

        public MainWindowMVVM()
        {
            shapes.Add(new IfShape(240, 80, Colors.Red, "IF"));        
            shapes.Add(new Sequence(240, 40, Colors.Blue, "Web"));  
        }
    }
}
