using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudioConcept.Extension
{
    public static class MouseEventArgExtension
    {
        /// <summary>
        /// determine mouse above(-1) or below(1)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="originPoint"></param>
        /// <param name="mileStone"></param>
        /// <returns></returns>
        public static int Direction(this MouseEventArgs e, Point originPoint, IInputElement mileStone)
        {
           return originPoint.Y - e.GetPosition(mileStone).Y >= 0 ? 1 : -1;
        }
    }
}
