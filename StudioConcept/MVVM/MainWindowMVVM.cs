using JetBrains.Annotations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;
using StudioConcept.Observer;
using IDropTarget = GongSolutions.Wpf.DragDrop.IDropTarget;

namespace StudioConcept.MVVM
{
    public class MainWindowMVVM : INotifyPropertyChanged, IDropTarget
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<BaseShape> shapes = new ObservableCollection<BaseShape>();
        private ObservableCollection<BaseShape> dragSr = new ObservableCollection<BaseShape>();

        public ObservableCollection<BaseShape> Shapes => shapes;
        public ObservableCollection<BaseShape> DragSr => dragSr;


        public MainWindowMVVM(IInputElement studio)
        {
            dragSr.Add(new Sequence(240, 30, Colors.Blue, "Web"));
            dragSr.Add(new IfShape(240, 30, Colors.Red, "If"));
            ObserverService.MileStone = studio;
            ObserverService.Parent = Shapes;
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

            BaseShape newItem = (BaseShape)item.Clone();
            newItem.X = drop.DropPosition.X;
            newItem.Y = drop.DropPosition.Y;
            newItem.IsRecorded = true;
            Shapes.Add(newItem);
        }
    }
}
