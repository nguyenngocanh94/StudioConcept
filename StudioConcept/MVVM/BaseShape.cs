using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudioConcept.Tree;


namespace StudioConcept.MVVM
{
    public class BaseShape : INotifyPropertyChanged, INode
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
                OnPropertyChanged(nameof(X));
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
                OnPropertyChanged(nameof(Y));
            }
        }

        private bool _isRecorded;
        public bool IsRecorded {
            get => _isRecorded;
            set
            {

                _isRecorded = value;
                OnPropertyChanged(nameof(IsRecorded));
            }
        }

        private List<BaseShape> _childrenNode;

        public List<BaseShape> ChildrenNode
            => _childrenNode ?? (_childrenNode = new List<BaseShape>());
    }
}
