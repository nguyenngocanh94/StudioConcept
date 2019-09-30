using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudioConcept.Command;
using StudioConcept.Observer;
using StudioConcept.Tree;
using System.Windows.Media.Effects;
using MediaColor = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace StudioConcept.MVVM
{
    public abstract class BaseShape : INotifyPropertyChanged, ICloneable, INode
    {
        public readonly int Id;
        #region for Base property
        private string _text;
        private bool _isRecorded;
        private double _innerX;
        public MediaColor Color { get; set; }
        private string _data;
        private double _innerY;
        private MediaColor _strokeColor;
        private bool _isNeedShadow;

        public State.ShapeState State { get; set; }
        private double _originY;
        protected double _middleSpace;

        public double MiddleSpace
        {
            get => _middleSpace;
            set
            {
                if (_middleSpace==value)
                {
                    return;
                }

                double delta = Math.Abs(value - _middleSpace);
                if (value >_middleSpace)
                {
                    Next?.DoAll(i => i.ChangeY(delta));
                    if (Parent != null)
                    {
                        Parent.MiddleSpace += delta;
                    }
                }
                else
                {
                    Next?.DoAll(i => i.ChangeY(0-delta));
                    if (Parent != null)
                    {
                        Parent.MiddleSpace -= delta;
                    }
                }

                
                _middleSpace = value;
                Data=Draw();
                VirtualHeight = Next?.VirtualHeight == null ? 0 : Next.VirtualHeight + Height;
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(VirtualHeight));
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

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public abstract string Draw();

        public double Width { get; set; }

        public virtual double Height { get; set; }

        // ReSharper disable once InconsistentNaming
        protected double _virtualHeight;
        public virtual double VirtualHeight
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_virtualHeight==0)
                {
                    return Height;
                }

                return _virtualHeight;
            }
            set
            {
                if (value==_virtualHeight)
                {
                    return;
                }

                _virtualHeight = value;
                OnPropertyChanged(nameof(VirtualHeight));
            }
        }

        /// <summary>
        /// this is the virtual highest point of a block.
        /// -(OuterUpperY) is the vertical axis
        /// *****************
        /// *     Block     *
        /// *****************
        /// </summary>
        public virtual double OuterUpperY => Y - Height/2;
        /// <summary>
        /// this is the virtual lowest point of a block.
        /// *****************
        /// *     Block     *
        /// *****************
        /// -(OuterUpperY) is the vertical axis.
        /// </summary>
        public virtual double VirtualOuterLowerY => VirtualInnerLowerY + Height/2;

        public virtual double OuterLowerY => InnerLowerY + Height / 2;
        /// <summary>
        /// the lowest but inner block.
        /// *****************
        /// *     Block     *
        /// ***************** => this is InnerLowerY,
        /// if current block is combine by multiple block,
        /// InnerLowerY is calculate by VirtualHeight
        /// </summary>
        public virtual double VirtualInnerLowerY => Y + VirtualHeight;
        public virtual double InnerLowerY => Y + Height;
        
        

        public virtual double ReCalculateVirtualHeight()
        {
            if (Next==null)
            {
                return Height;
            }

            double height = 0;
            DoAll(i => { height += i.Height;});

            return height;
        }

        public virtual void ArrangeChildrenPosition() { }

        #region Drag
        /// <summary>
        /// Check if other is bellow range.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAtBottom(BaseShape other)
        {
            return Math.Abs(X - other.X) < 40 && OuterUpperY < other.OuterLowerY &&
                   OuterUpperY > other.Y;
        }
        /// <summary>
        /// Check if in above range.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAtAbove(BaseShape other)
        {
           return  Math.Abs(X - other.X) < 40 && VirtualOuterLowerY > other.OuterUpperY && Y < other.Y;
        }
        /// <summary>
        /// Check if is middle of an IBranch shape
        /// </summary>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public bool IsAtMiddle(BaseShape otherShape)
        {
            if (otherShape is IBranch)
            {
                return Math.Abs(X - otherShape.X) < 60 && Y > otherShape.Y && Y < otherShape.VirtualInnerLowerY;
            }
            return false;
        }
        #endregion

        #region Coordinate
        private double _x;
        private double _y;
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
                var delta = Math.Abs(value - _x);
                // drag down
                if (value > _x)
                {
                    foreach (var child in ChildrenNode)
                    {
                        child.X += delta;
                    }
                }
                // drag up
                else
                {
                    foreach (var child in ChildrenNode)
                    {
                        child.X -= delta;
                    }
                }

                _x = value;

                OnPropertyChanged(nameof(X));
            }
        }

        public void SetCoordinate(double valueX, double valueY)
        {
            var delta = Math.Abs(valueY - Y);
            X = valueX;
            Y = valueY;
            State = Observer.State.ShapeState.Balance;
            _originY = Y;
            DoAllWithoutHead(i =>
            {
                i._originY = i.Y;
                i.State = Observer.State.ShapeState.Balance;
                i.X = valueX;
                if (valueY > Y)
                {
                    i.Y += delta;
                }
                else
                {
                    i.Y -= delta;
                }
            });
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
                var delta = Math.Abs(value - _y);
                // drag down
                if (value > _y)
                {
                    foreach (var child in ChildrenNode)
                    {
                        child.Y += delta;
                    }
                }
                // drag up
                else
                {
                    foreach (var child in ChildrenNode)
                    {
                        child.Y -= delta;
                    }
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



        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
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

        public ObserverService TrackingService;
        public ICommand MouseUpCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }

        // ReSharper disable once InconsistentNaming
        private Point originPoint;
        private bool _isLeftMouseDownOnShape;
        private bool _isDragging;

        protected BaseShape()
        {
            MouseUpCommand = new BaseCommand(re =>
            {
                var source = (FrameworkElement)re.Source;
                var mvvmSource = (BaseShape)source.DataContext;
                mvvmSource.TrackingService?.Magnet();
                source.Effect = null;
                source.ReleaseMouseCapture();
                _isLeftMouseDownOnShape = false;
                _isDragging = false;
                re.Handled = true;
            });
            MouseMoveCommand = new BaseCommand(re =>
            {
                var mvvmSource = (BaseShape)((FrameworkElement)re.Source).DataContext;
                if (_isDragging)
                {
                    Point currentPoint = re.GetPosition(ObserverService.MileStone);
                    var dragDelta = currentPoint - originPoint;
                    if (dragDelta != new Vector(0, 0))
                    {
                        mvvmSource.TrackingService?.Tracking();
                        mvvmSource.Move(dragDelta);
                        originPoint = currentPoint;
                    }

                }

                else if (_isLeftMouseDownOnShape)
                {
                    Point currentPoint = re.GetPosition(ObserverService.MileStone);
                    var dragDelta = currentPoint - originPoint;
                    double dragDistance = dragDelta.Length;
                    if (dragDistance > 5)
                    {
                        _isDragging = true;
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
                if (mvvmSource.TrackingService == null)
                {
                    mvvmSource.TrackingService = new ObserverService(mvvmSource);
                    mvvmSource.TrackingService.WarningEventHandler += (s, e) =>
                    {
                        mvvmSource.StrokeColor = MediaColor.FromRgb(255, 255, 255);
                        e.StrokeColor = MediaColor.FromRgb(255, 255, 255);
                    };
                }

                source.CaptureMouse();
                originPoint = re.GetPosition(ObserverService.MileStone);
                re.Handled = true;
                _isLeftMouseDownOnShape = true;
            });
            ChildrenNode.CollectionChanged += UpdateMiddleSpaceWhenChildrenChanged;
            StrokeColor = MediaColor.FromArgb(255, 255, 255, 255);
            Id = (int)DateTime.Now.TimeOfDay.TotalMilliseconds;
            Count = 1;
            State = Observer.State.ShapeState.Balance;
        }

        private void Move(Vector distance)
        {
            DoAll(i =>
            {
                i.X += distance.X;
                i.Y += distance.Y;
            });
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

        #region utilities

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

        /// <summary>
        /// Do All in linked list Until predicate return true
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool DoAll(Predicate<BaseShape> predicate)
        {
            if (IsTail())
            {
                if (predicate(this))
                {
                    return true;
                }
            }
            else
            {
                var current = this;
                while (current != null)
                {
                    if (predicate(current))
                    {
                        return true;
                    }
                    current = current.Next;
                }
            }

            return false;
        }

        public void DoInverseWithoutCurrent(Action<BaseShape> action)
        {
            if (IsHead())
            {
                return;
            }

            var current = Prev;
            while (current!=null)
            {
                action(current);
                current = current.Prev;
            }
        }

        public void DoInverseAll(Action<BaseShape> action)
        {
            action(this);
            DoInverseWithoutCurrent(action);
        }

        public void DoAll(Action<BaseShape> action)
        {
            action(this);
            DoAllWithoutHead(action);
        }

        public void InsertTail(BaseShape other)
        {
            // is tails so update the inverse.
            if (IsTail())
            {
                Next = other;
                other.Prev = this;
            }
            else
            {
                var tail = GetTail();
                tail.InsertTail(other);
            }
            
        }

        public void InsertHead(BaseShape other)
        {
            other.DoAll(i => i.VirtualHeight += VirtualHeight);
            var otherTail = other.GetTail();
            otherTail.Next = this;
            Prev = otherTail;
        }

        public int RemoveTail(BaseShape other)
        {
            if (Next!=other||Next==null)
            {
                return -1;
            }

            DoInverseAll(i=>i.VirtualHeight -= other.VirtualHeight);
            return 1;
        }
        #endregion

        #region for the drag drop animation
        public void Down(double delta)
        {
            if (State == Observer.State.ShapeState.Balance)
            {
                _originY = _y;
                Y += delta;
                State = Observer.State.ShapeState.Down;
            }
        }

        public void ChangeY(double delta)
        {
            if (State == Observer.State.ShapeState.Balance|| State == Observer.State.ShapeState.Changed)
            {
                _originY = _y;
                Y += delta;
                State = Observer.State.ShapeState.Changed;
            }
        }

        public virtual void Expand(double delta) { }

        public void Redo()
        {
            if (State != Observer.State.ShapeState.Balance&&State!=Observer.State.ShapeState.Expand)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_originY != 0)
                {
                    Y = _originY;
                }
                State = Observer.State.ShapeState.Balance;
            }
            
        }


        #endregion

        #region for TreeView
        [CanBeNull] public BaseShape Parent { get; set; }

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
        }

        #endregion

        #region For LinkedList

        public int Count { get; set; }
        private BaseShape _next;

        [CanBeNull]
        public BaseShape Next
        {
            get => _next;
            set
            {
                if (value==null)
                {
                    Count = 1;
                    VirtualHeight = Height;
                    if (_next!=null)
                    {
                        DoInverseWithoutCurrent(i => i.Count -= _next.Count);
                        DoInverseWithoutCurrent(i => i.VirtualHeight -= _next.VirtualHeight);
                    }
                }
                else
                {
                    Count += value.Count;
                    VirtualHeight += value.VirtualHeight;
                    DoInverseWithoutCurrent(i => i.Count += value.Count);
                    DoInverseWithoutCurrent(i => i.VirtualHeight += value.VirtualHeight);
                }
                _next = value;
            }
        }

        [CanBeNull]
        public BaseShape Prev { get; set; }

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
            // ReSharper disable once PossibleNullReferenceException
            while (!head.IsHead())
            {
                head = head.Prev;
            }

            return head;
        }

        public BaseShape GetTail()
        {
            var tail = this;
            // ReSharper disable once PossibleNullReferenceException
            while (!tail.IsTail())
            {
                tail = tail.Next;
            }

            return tail;
        }
        public bool HeadOf(BaseShape other)
        {
            var current = this;
            while (current != null)
            {
                if (current == other)
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public bool IsDescendents(BaseShape other)
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent == other)
                {
                    return true;
                }
                parent = parent.Parent;
            }

            return false;
        }

        public bool HasDescendents(BaseShape other)
        {
            return other.IsDescendents(this);
        }

        public void Alone()
        {
            Parent?.ChildrenNode.Remove(this);
            Parent = null;
        }

        public void RemoveHead()
        {
            if (Prev != null) Prev.Next = null;
            Prev = null;
        }

        #endregion

        #region for the converter
        public double InnerY
        {
            get => _innerY;
            set
            {

                _innerY = value;
                OnPropertyChanged(nameof(InnerY));
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

            ((BaseShape)cloned)._data = Draw();

            return cloned;
        }

        public void ResetStroke()
        {
            //StrokeColor = MediaColor.FromArgb(100, Color.R, Color.G, Color.B);
            StrokeColor = MediaColor.FromArgb(255, 255, 255, 255);;
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseShape shape)
            {
                return Id == shape.Id;
            }

            return false;
        }
    }
}
