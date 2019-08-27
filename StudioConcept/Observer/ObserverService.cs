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

            //prioritize for drag to bottom
            var temp = interactSet.FirstOrDefault(i =>
            {
                if (Math.Abs(pivot.X - i.X) < 60 && i is IfShape && pivot.Y > ((IfShape)i).MiddleUpperY && pivot.Y < ((IfShape)i).MiddleLowerY)
                {
                    interact = null;
                    interact += MagnetMiddle;
                    Console.WriteLine("Catch pivot " + pivot.Text + " (" + pivot.X + " " + pivot.Y + ")" + " drag to top shape: " + i.Text + " (" + i.X + " " + i.Y + ")");
                    return true;
                }
                // drag to bottom of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterUpperY < i.OuterLowerY && pivot.OuterUpperY > i.Y)
                {
                    interact = null;
                    interact += MagnetBot;
                    Console.WriteLine("Catch pivot "+pivot.Text+" ("+pivot.X+" "+pivot.Y+")" +" drag to bottom shape: "+i.Text + " (" + i.X + " " + i.Y + ")");
                    return true;
                }
                // drag to above of other
                if (Math.Abs(pivot.X - i.X) < 40 && pivot.OuterLowerY > i.OuterUpperY && pivot.Y < i.Y)
                {
                    interact = null;
                    interact += MagnetTop;
                    Console.WriteLine("Catch pivot " + pivot.Text + " (" + pivot.X + " " + pivot.Y + ")" + " drag to top shape: " + i.Text + " (" + i.X + " " + i.Y + ")");
                    return true;
                }

                
                return false;
            });

            if (temp!=null)
            {
                if (temp!=otherShape&&otherShape!=null)
                {
                    if (temp.GetHead()==otherShape.GetHead())
                    {
                        Console.WriteLine("current and old interact is in same DLL");
                        if (!temp.IsTail())
                        {
                            Console.WriteLine("current is not tail");
                            if (!temp.IsHead())
                            {
                                Console.WriteLine("current is not head");
                                //temp.Y += pivot.Height+8.8;
                                temp.DoAllWithoutHead(i => i.Y += pivot.Height + 8.8);
                            }
                            else
                            {
                                Console.WriteLine("current is head");
                                //drag pivot to bottom of a head
                                if (temp.Y < pivot.Y)
                                {
                                    temp.DoAllWithoutHead(i => i.Y += pivot.Height + 8.8);
                                }
                                //drag to top of a head, do nothing
                            }
                            
                        }
                        otherShape.DoAllWithoutHead(i=>i.Y-=pivot.Height+8.8);
                    }
                    else
                    {
                        otherShape.DoAllWithoutHead(i => i.Y -= pivot.Height+8.8);
                    }

                }

                warning?.Invoke(this, temp);
                otherShape = temp;
            }
            else
            {
                interact = null;
                RemoveRelation();
                pivot.StrokeColor = transparent;
                if (otherShape!=null)
                {
                    if (!otherShape.IsTail())
                    {
                        otherShape.DoAllWithoutHead(i => { i.Y -= 40;});
                    }
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

           
            //determine relationship for pivot and its new head.
            if (other.IsTail())
            {
                // other is a single
                //calculate the position of pivot.
                pivot.X = other.X;
                var delta = Math.Abs(other.InnerLowerY + 8.8 - pivot.Y);
                pivot.Y = other.InnerLowerY + 8.8;
                //calculate the next shape head by pivot.
                pivot.DoAllWithoutHead(i =>
                {
                    i.X = other.X;
                    i.Y -= delta;
                });
            }
            else
            {
                // when drag pivot in middle of two shape

                //calculate the position of pivot.
                pivot.X = other.X;
                var delta = Math.Abs(other.InnerLowerY + 8.8 - pivot.Y);
                pivot.Y = other.InnerLowerY + 8.8;
                
                double totalHeight = 0;
                pivot.DoAllWithoutHead(i =>
                {
                    i.X = other.X;
                    i.Y -= delta;
                    totalHeight += i.Height + 8.8;
                });
                other.Next.Y += totalHeight;

                //calculate the next shape head by pivot.
                var next = other.Next;
                next.Prev = pivot.GetTail();
                pivot.GetTail().Next = next;
                
            }
            other.Next = pivot;
            pivot.Prev = other;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
           
            Console.WriteLine("MagnetBot: pivot - "+ pivot.Text + " (" + pivot.X + " " + pivot.Y + ")"+ " ,other - " + otherShape.Text + " (" + otherShape.X + " " + otherShape.Y + ")");
            otherShape = null;
        }

        private void MagnetTop(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            //calculate the position of pivot.
            pivot.X = other.X;
            var delta = Math.Abs(other.InnerLowerY + 8.8 - pivot.Y);
            pivot.Y = other.Y - pivot.Height - 8.8;
            //calculate the next shape head by pivot.
            pivot.DoAllWithoutHead(i =>
            {
                i.X = other.X;
                i.Y += delta;
            });
            //determine relationship for pivot and its new head.
            pivot.Next = other;
            other.Prev = pivot;
            pivot.StrokeColor = transparent;
            other.StrokeColor = transparent;
            
            Console.WriteLine("MagnetTop: pivot - " + pivot.Text + " (" + pivot.X + " " + pivot.Y + ")" + " ,other - " + otherShape.Text + " (" + otherShape.X + " " + otherShape.Y + ")");
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

        private void RemoveRelation()
        {
            var temp = pivot.Prev;
            pivot.Prev = null;
            if (temp!=null)
            {
                temp.Next = null;
            }
        }

    }
}
