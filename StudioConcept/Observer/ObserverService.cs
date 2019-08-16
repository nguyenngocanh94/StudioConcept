using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using StudioConcept.MVVM;

namespace StudioConcept.Observer
{
    public class ObserverService
    {
        public static ObservableCollection<BaseShape> Parent;
        public static IInputElement MileStone;

        public static void Observer(BaseShape listener, int direction)
        {
            if (Parent.Count < 2)
            {
                return;
            }

            if (direction==-1)
            {
                var res = Parent.FirstOrDefault(i =>
                {
                    if (listener.UpperY > i.Y && listener.UpperY < i.LowerY)
                    {
                        if (Math.Abs(listener.X - i.X) < 40)
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }
            else
            {
                var res = Parent.FirstOrDefault(i =>
                {
                    var lower = listener.LowerY + listener.Height;
                    if (lower > i.Y && lower < i.LowerY)
                    {
                        if (Math.Abs(listener.X - i.X) < 40)
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }

            

        }
    }
}
