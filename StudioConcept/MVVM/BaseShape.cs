using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using StudioConcept.Command;
using StudioConcept.Extension;
using StudioConcept.Observer;
using StudioConcept.Tree;


namespace StudioConcept.MVVM
{
    public abstract class BaseShape : INotifyPropertyChanged, INode, ICloneable
    {

        public double Width { get; set; }
        public virtual double Height { get; set; }

        public abstract string Draw();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual double UpperY => Y - Height;
        public virtual double LowerX => Y + Height;
        private double _x;
        public double X
        {
            get => _x;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_x == value)
                {
                    return;
                }
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_y == value)
                {
                    return;
                }
                _y = value;
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

        private double _innerX;
        public double InnerX {
            get => _innerX;
            set
            {

                _innerX = value;
                OnPropertyChanged(nameof(InnerX));
            }
        }

        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private double _innerY;
        public double InnerY
        {
            get => _innerY;
            set
            {

                _innerY = value;
                OnPropertyChanged(nameof(InnerY));
            }
        }

        public ICommand MouseUpCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        // ReSharper disable once InconsistentNaming
        private System.Windows.Point originPoint;
        private bool isLeftMouseDownOnShape;
        private bool isDragging;

        public BaseShape()
        {
            MouseUpCommand = new BaseCommand(re =>
            {
                var source = (FrameworkElement)re.Source;
                source.ReleaseMouseCapture();
                isLeftMouseDownOnShape = false;
                isDragging = false;
                re.Handled = true;
            });
            MouseMoveCommand = new BaseCommand(re =>
            {
                var mvvmSource = (BaseShape)((FrameworkElement)re.Source).DataContext;
                if (isDragging)
                {
                    System.Windows.Point currentPoint = re.GetPosition(ObserverService.MileStone);
                    var dragDelta = currentPoint - originPoint;
                    

                    mvvmSource.X += dragDelta.X;
                    mvvmSource.Y += dragDelta.Y;

                    ObserverService.Observer(mvvmSource, re.Direction(originPoint, ObserverService.MileStone));
                    originPoint = currentPoint;
                }

                else if (isLeftMouseDownOnShape)
                {
                    System.Windows.Point currentPoint = re.GetPosition(ObserverService.MileStone);
                    var dragDelta = currentPoint - originPoint;
                    double dragDistance = dragDelta.Length;
                    if (dragDistance > 5)
                    {
                        isDragging = true;
                    }

                    re.Handled = true;
                }
            });
            MouseDownCommand = new BaseCommand(re =>
            {
                var source = (FrameworkElement)re.Source;
                source.CaptureMouse();
                originPoint = re.GetPosition(ObserverService.MileStone);
                re.Handled = true;
                isLeftMouseDownOnShape = true;
            });
        }

        private List<BaseShape> _childrenNode;

        public List<BaseShape> ChildrenNode
            => _childrenNode ?? (_childrenNode = new List<BaseShape>());

        public object Clone()
        {
            var type = GetType();
            var cloned = Activator.CreateInstance(type);
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                
                if (prop.CanWrite)
                {
                    prop.SetValue(cloned, prop.GetValue(this, null));
                }
            }

            ((BaseShape) cloned)._data = Draw();
            
            return cloned;
        }
    }
}
