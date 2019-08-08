using System;
using JetBrains.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;
using StudioConcept.ActionItem;
using Control = System.Windows.Forms.Control;
using IDropTarget = GongSolutions.Wpf.DragDrop.IDropTarget;

namespace StudioConcept.MVVM
{
    class MainWindowMVVM : INotifyPropertyChanged, IDropTarget
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isLeftMouseDown;
        //Records the location of the mouse 
        //  (relative to the window) when the
        // left-mouse button has pressed down
        private Point origMouseDownPoint;
        /// <summary>
        /// Set to 'true' when dragging a rectangle.
        /// </summary>
        private bool isDraggingRectangle = false;
        private ObservableCollection<BaseShape> shapes = new ObservableCollection<BaseShape>();
        private ObservableCollection<BaseShape> dragSr = new ObservableCollection<BaseShape>();

        public ObservableCollection<BaseShape> Shapes => shapes;
        public ObservableCollection<BaseShape> DragSr => dragSr;

        public Canvas dropTarget;

        public MainWindowMVVM(Canvas dropTarget)
        {
            shapes.Add(new Sequence(240, 40, Colors.Blue, "Web"));
            this.dropTarget = dropTarget;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var drop = (DropInfo) dropInfo;
            var item = (BaseShape)drop.Data;
            if (item.IsRecorded)
            {
                item.X = drop.DropPosition.X;
                item.Y = drop.DropPosition.Y;
            }
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var drop = (DropInfo) dropInfo;
            var item = (BaseShape) dropInfo.Data;
            item.X = drop.DropPosition.X;
            item.Y = drop.DropPosition.Y;
            item.IsRecorded = true;
            var a = new SequenceAction();
            a.MouseEnter += (s, e) => { Console.WriteLine(s.ToString()); };
            a.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != MouseButton.Left)
                {
                    return;
                }

                var shape = (FrameworkElement) s;
                isLeftMouseDown = true;
                shape.CaptureMouse();
                origMouseDownPoint = e.GetPosition(dropTarget);
                e.Handled = true;
            };

            a.MouseUp += (s, e) =>
            {
                if (isLeftMouseDown)
                {
                    var shape = (FrameworkElement)s;
                    shape.ReleaseMouseCapture();
                    isLeftMouseDown = false;
                    e.Handled = true;
                }

                isDraggingRectangle = false;

            };
            a.MouseMove += (s, e) =>
            {
                if (isDraggingRectangle)
                {
                    Point curMouseDownPoint = e.GetPosition(dropTarget);
                    var dragDelta = curMouseDownPoint - origMouseDownPoint;
                    origMouseDownPoint = curMouseDownPoint;
                    Canvas.SetTop(a, curMouseDownPoint.Y);
                    Canvas.SetLeft(a, curMouseDownPoint.X);
                }else if (isLeftMouseDown)
                {
                    Point curMouseDownPoint = e.GetPosition(a);
                    var dragDelta = curMouseDownPoint - origMouseDownPoint;
                    double dragDistance = Math.Abs(dragDelta.Length);
                    if (dragDistance > 5)
                    {
                        //
                        // When the mouse has been dragged more than the threshold value commence dragging the rectangle.
                        //
                        isDraggingRectangle = true;
                    }
                    e.Handled = true;
                }
            };
            a.DataContext = item;
            dropTarget.Children.Add(a);
            Canvas.SetTop(a, item.Y);
            Canvas.SetLeft(a,item.X);
        }
    }
}
