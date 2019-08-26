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
        private BaseShape otherShape;
        private HashSet<BaseShape> interactedList;
        public EventHandler<BaseShape> interact;
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
            // remove itself and its child.
            var interactSet = Parent.Where(i =>
            {
                if (i == pivot)
                {
                    return false;
                }

                if (pivot.Contain(i))
                {
                    return false;
                }

                return true;
            }).ToList();

            var temp = interactSet.FirstOrDefault(i =>
            {
                
                // drag to bottom of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterUpperY < i.OuterLowerY && pivot.OuterUpperY > i.Y)
                {
                    interact = null;
                    interact += MagnetBot;
                    return true;
                }
                // drag to above of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterLowerY > i.OuterUpperY && pivot.Y < i.Y)
                {
                    interact = null;
                    interact += MagnetTop;
                    return true;
                }
                // drag to middle of other, only for shape that have child
                if(Math.Abs(pivot.X - i.X) < 40 && i is IfShape && pivot.Y > i.Y && pivot.InnerLowerY < i.InnerLowerY)
                {
                   
                    return true;
                }

                return false;
            });

            if (temp!=null)
            {
                if (!temp.IsTail())
                {
                    interact += MagnetMiddle;
                    otherShape = temp;
                }
                if (otherShape!=null)
                {
                    if (otherShape!=temp)
                    {
                        otherShape.StrokeColor = transparent;
                        warning?.Invoke(this, temp);
                        otherShape = temp;
                    }
                }
                else
                {
                    otherShape = temp;
                    warning?.Invoke(this, temp);
                }
            }
            else
            {
                interact = null;
                RemoveLinked();
                pivot.StrokeColor = transparent;
                if (otherShape!=null)
                {
                    otherShape.StrokeColor = transparent;
                    otherShape = null;
                }
            }

        }

        public void Magnet()
        {
            interact?.Invoke(this, otherShape);
            interact = null;
        }

        private void MagnetBot(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            
            pivot.X = other.X;
            var delta = Math.Abs(other.InnerLowerY + 8.8 - pivot.Y);
            pivot.Y = other.InnerLowerY + 8.8;
            
            var temp = pivot.Next;
            while (temp != null)
            {
                temp.X = other.X;
                temp.Y -= delta;
                temp = temp.Next;
            }
            pivot.Prev = other;
            other.Next = pivot;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
            otherShape = null;
        }

        private void MagnetTop(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            
           
            pivot.X = other.X;
            var delta = Math.Abs(other.InnerLowerY + 8.8 - pivot.Y);
            pivot.Y = other.Y - pivot.Height - 8.8;
            var temp = pivot.Next;
            while (temp != null)
            {
                temp.X = other.X;
                temp.Y += delta;
                temp = temp.Next;
            }
            pivot.Next = other;
            other.Prev = pivot;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
            otherShape = null;
        }

        private void MagnetMiddle(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            var next = other.Next;
            pivot.X = other.X;
            pivot.Y = other.InnerLowerY + 8.8;
            next.Y = pivot.InnerLowerY + 8.8;
            pivot.Prev = other;
            pivot.Next = other.Next;
            other.Next = pivot;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
        }

        private void RemoveLinked()
        {
            var prev = pivot.Prev;
            if (prev != null)
            {
                prev.Next = null;
            }
            pivot.Prev = null;
        }

    }
}
