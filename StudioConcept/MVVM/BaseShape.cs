using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudioConcept.Command;
using StudioConcept.Observer;
using StudioConcept.Tree;
using System.Windows.Media.Effects;
using StudioConcept.Extension;
using MediaColor = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace StudioConcept.MVVM
{
    public abstract class BaseShape : INotifyPropertyChanged, ICloneable, INode
    {
        #region for Base property
        private string _text;
        private double _x;
        private double _y;
        private bool _isRecorded;
        private double _innerX;
        private string _data;
        private double _innerY;
        private MediaColor _strokeColor;
        private bool _isNeedShadow;
        protected double _middleSpace;
        public ShapeState State = ShapeState.Balance;
        private double _originY;
        
        public void Down(double delta)
        {
            if (State== ShapeState.Down)
            {
                Console.WriteLine(Text+" CURRENT STATUS: DOWN");
            }
            if (State == ShapeState.Balance)
            {
                Console.WriteLine(Text + " CURRENT STATUS: BALANCE");
                _originY = Y;
                State = ShapeState.Down;
                Y += delta;
            }
        }

        public void Redo()
        {
            if (State!= ShapeState.Balance)
            {
                Y = _originY;
            }
            Console.WriteLine(Text+" REDO TO BALANCE");
            State = ShapeState.Balance;
        }

        public double MiddleSpace
        {
            get => _middleSpace;
            set
            {
                _middleSpace = value; Data = Draw();

            }
        }
        public bool IsNeedShadow
        {
            get => _isNeedShadow;
            set
            {
                _isNeedShadow = value;
                OnPropertyChanged(nameof(IsNeedShadow));
            }
        }

        private BaseShape _next;
        public BaseShape Next
        {
            get=> _next;
            set => _next = value;
        }

        private BaseShape _prev;

        public BaseShape Prev
        {
            get => _prev;
            set => _prev = value;
        }

        public bool IsTail()
        {
            return Next == null;
        }

        public bool IsHead()
        {
            return Prev == null;
        }

        public bool IsAlone()
        {
            return Prev == null && Next == null;
        }

        public BaseShape GetHead()
        {
            var head = this;
            while (!head.IsHead())
            {
                head = head.Prev;
            }

            return head;
        }

        public BaseShape GetTail()
        {
            var tail = this;
            while (!tail.IsTail())
            {
                tail = tail.Next;
            }

            return tail;
        }
        
        public void DoAllWithoutHead(Action<BaseShape> action)
        {
            if (IsTail())
            {
                return;
            }
            var current = Next;
            while (current != null)
            {
                action(current);
                current = current.Next;
            }
        }

        public void DoAll(Action<BaseShape> action)
        {
            action(this);
            DoAllWithoutHead(action);
        }

        public bool HeadOf(BaseShape other)
        {
            var current = this;
            while (current!=null)
            {
                if (current==other)
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public double Width { get; set; }
        public virtual double Height { get; set; }
        public abstract string Draw();
        public virtual double OuterUpperY => Y - Height/2;
        public virtual double OuterLowerY => InnerLowerY + Height/2;
        public virtual double InnerLowerY => Y + Height;
        
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

        public bool IsRecorded
        {
            get => _isRecorded;
            set
            {

                _isRecorded = value;
                OnPropertyChanged(nameof(IsRecorded));
            }
        }

        public double InnerX
        {
            get => _innerX;
            set
            {

                _innerX = value;
                OnPropertyChanged(nameof(InnerX));
            }
        }

        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public double InnerY
        {
            get => _innerY;
            set
            {

                _innerY = value;
                OnPropertyChanged(nameof(InnerY));
            }
        }

        public MediaColor StrokeColor
        {
            get => _strokeColor;
            set
            {

                _strokeColor = value;
                OnPropertyChanged(nameof(StrokeColor));
            }
        }
        #endregion

        public ObserverService trackingService;
        public ICommand MouseUpCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        // ReSharper disable once InconsistentNaming
        private Point originPoint;
        private bool isLeftMouseDownOnShape;
        private bool isDragging;

        public BaseShape()
        {
            StrokeColor = MediaColor.FromArgb(0, 0, 0, 0);
            MouseUpCommand = new BaseCommand(re =>
            {
                var source = (FrameworkElement)re.Source;
                var mvvmSource = (BaseShape)source.DataContext;
                mvvmSource.trackingService?.Magnet();
                source.Effect = null;
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
                    Point currentPoint = re.GetPosition(ObserverService.MileStone);
                    var dragDelta = currentPoint - originPoint;
                    if (dragDelta != new Vector(0,0))
                    {
                        mvvmSource.trackingService?.Tracking();
                        mvvmSource.X += dragDelta.X;
                        mvvmSource.Y += dragDelta.Y;
                        foreach (var baseShape in mvvmSource.ChildrenNode)
                        {
                            baseShape.X += dragDelta.X;
                            baseShape.Y += dragDelta.Y;
                        }
                        var temp = mvvmSource.Next;
                        while (temp!=null)
                        {
                            temp.X+= dragDelta.X;
                            temp.Y+= dragDelta.Y;
                            foreach (var baseShape in temp.ChildrenNode)
                            {
                                baseShape.X += dragDelta.X;
                                baseShape.Y += dragDelta.Y;
                            }
                            temp = temp.Next;
                        }
                        originPoint = currentPoint;
                    }
                    
                }

                else if (isLeftMouseDownOnShape)
                {
                    Point currentPoint = re.GetPosition(ObserverService.MileStone);
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
                var mvvmSource = (BaseShape)source.DataContext;
                source.Effect = new DropShadowEffect()
                {
                    Color = MediaColor.FromRgb(192, 192, 192),
                    Direction = 1,
                    Opacity = 0.6

                };
                mvvmSource.IsNeedShadow = true;
                if (mvvmSource.trackingService==null)
                {
                    mvvmSource.trackingService = new ObserverService(mvvmSource);
                    mvvmSource.trackingService.WarningEventHandler += (s, e) =>
                    {
                        mvvmSource.StrokeColor = MediaColor.FromRgb(255, 255, 255);
                        e.StrokeColor = MediaColor.FromRgb(255, 255, 255);
                    };
                }
                
                source.CaptureMouse();
                originPoint = re.GetPosition(ObserverService.MileStone);
                re.Handled = true;
                isLeftMouseDownOnShape = true;
            });
            ChildrenNode.CollectionChanged += UpdateMiddleSpaceWhenChildrenChanged;
        }
        #region for treeview

        private BaseShape _parent;

        public BaseShape Parent { get; set; }

        private ObservableCollection<BaseShape> _childrenNode;

        public ObservableCollection<BaseShape> ChildrenNode
            => _childrenNode ?? (_childrenNode = new ObservableCollection<BaseShape>());

        private void UpdateMiddleSpaceWhenChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // ReSharper disable once MergeCastWithTypeCheck
            if (sender is ObservableCollection<BaseShape>)
            {
                if (((ObservableCollection<BaseShape>)sender).Count == 0)
                {
                    MiddleSpace = 20;
                    return;
                }
            }

            UpdateMiddleSpace();
        }
        public virtual void UpdateMiddleSpace()
        {
            double middleSpace = 0;
            
            foreach (var baseShape in ChildrenNode)
            {
                middleSpace += baseShape.Height + 8.8;
            }

            MiddleSpace = middleSpace - 8.8;
        }

        #endregion
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


        #region Drag
        /// <summary>
        /// Check if other is bellow range.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAtBottom(BaseShape other)
        {
            return Math.Abs(this.X - other.X) < 40 && this.OuterUpperY < other.OuterLowerY &&
                   this.OuterUpperY > other.Y;
        }
        /// <summary>
        /// Check if in above range.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAtAbove(BaseShape other)
        {
            return Math.Abs(X - other.X) < 40 && OuterLowerY > other.OuterUpperY && Y < other.Y;
        }
        /// <summary>
        /// Check if is middle of an IBranch shape
        /// </summary>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public bool IsAtMiddle(BaseShape otherShape)
        {
            if (!(otherShape is IBranch))
            {
                return false;
            }

            return Math.Abs(X - otherShape.X) < 60 && Y > ((IfShape) otherShape).Y && Y < ((IfShape) otherShape).InnerLowerY;
        }
        #endregion

        #region for INotify
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
