using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediaColor = System.Windows.Media.Color;
using System.Windows;
using StudioConcept.MVVM;

namespace StudioConcept.Observer
{
    public class ObserverService
    {
        public MediaColor transparent = MediaColor.FromArgb(0, 0, 0, 0);
        public static ObservableCollection<BaseShape> Parent;
        public static IInputElement MileStone;
        private BaseShape pivot;
        private BaseShape interact;
        private HashSet<BaseShape> interactedList;
        public EventHandler<BaseShape> magnet;
        public EventHandler<BaseShape> warning;
        public ObserverService(BaseShape pivot)
        {
            this.pivot = pivot;
            interactedList = new HashSet<BaseShape>();
        }

        public void Tracking()
        {
            if (Parent.Count < 2)
            {
                return;
            }

            var temp = Parent.FirstOrDefault(i =>
            {
                // drag to bottom of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterUpperY < i.OuterLowwerY && pivot.OuterUpperY > i.Y)
                {
                    magnet = null;
                    magnet += MagnetBot;
                    return true;
                }
                // drag to above of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterLowwerY > i.OuterUpperY && pivot.Y < i.Y)
                {
                    magnet = null;
                    magnet += MagnetTop;
                    return true;
                }
                // drag to middle of other, only for shape that have child
//                if(Math.Abs(pivot.X - i.X) < 40 && i is IfShape && pivot.Y > i.Y && pivot.InnerLowerY < i.InnerLowerY)
//                {
//                   
//                    return true;
//                }

                return false;
            });

            if (temp!=null)
            {
                if (interact!=null)
                {
                    if (interact!=temp)
                    {
                        interact.StrokeColor = transparent;
                        warning?.Invoke(this, temp);
                        interact = temp;
                    }
                }
                else
                {
                    interact = temp;
                    warning?.Invoke(this, temp);
                }
            }
            else
            {
                magnet = null;
                pivot.groupList.Clear();
                pivot.StrokeColor = transparent;
                if (interact!=null)
                {
                    interact.StrokeColor = transparent;
                    interact = null;
                }
            }

        }

        public void Magnet()
        {
            magnet?.Invoke(this, interact);
        }

        private void MagnetBot(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            other.next = pivot;
            interactedList.Add(other);
            pivot.X = other.X;
            pivot.Y = other.InnerLowerY + 8.8;
            var temp = pivot.next;
            while (temp != null)
            {
                temp.X = other.X;
                temp.Y = pivot.Y + other.InnerLowerY + 8.8;
                temp = temp.next;
            }
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
        }

        private void MagnetTop(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            pivot.next = other;
            interactedList.Add(other);
            
            pivot.X = other.X;
            pivot.Y = other.Y - pivot.Height - 8.8;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
        }

        private void MagnetMiddle(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            pivot.X = other.X;
            pivot.Y = other.InnerLowerY + 8.8;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
        }

        public void RemoveRelation()
        {
            foreach (var shape in interactedList)
            {
                if (shape!=interact)
                {
                    if (shape.groupList.Contains(pivot))
                    {
                        shape.groupList.Remove(pivot);
                        break;
                    }
                }
            }
        }

    }
}
