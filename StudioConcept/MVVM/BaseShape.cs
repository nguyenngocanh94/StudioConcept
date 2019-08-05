using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace StudioConcept.MVVM
{
    public class BaseShape : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double x;
        public double X
        {
            get => x;
            set
            {
                if (x == value)
                {
                    return;
                }
                x = value;
                OnPropertyChanged("X");
            }
        }

        private double y;
        public double Y
        {
            get => y;
            set
            {
                if (y == value)
                {
                    return;
                }
                y = value;
                OnPropertyChanged("Y");
            }
        }
    }
}
