using System.Windows;
using System.Timers;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace StudioConcept.Extension
{
    public static class MouseEventArgExtension
    {
        private static Timer aTimer;
        /// <summary>
        /// determine mouse above(-1) or below(1)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="originPoint"></param>
        /// <param name="mileStone"></param>
        /// <returns></returns>
        public static int Direction(this MouseEventArgs e, Point originPoint, IInputElement mileStone)
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            double delta = 0;
            aTimer.Elapsed += (s, ei) =>
            {
                delta = originPoint.Y - e.GetPosition(mileStone).Y;
            };

            return delta >= 0 ? 1 : -1;
        }
    }
}
