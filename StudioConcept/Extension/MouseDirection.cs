using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudioConcept.Extension
{
    public enum MouseDirection
    {
        None,
        Up,
        Down,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public class MouseMovement
    {
        public static MouseDirection GetMouseDirection(Point pre, Point cur)
        {
            // Mouse moved up
            if ((pre.X == cur.X) && (pre.Y > cur.Y))
                return MouseDirection.Up;

            // Mouse moved down
            if ((pre.X == cur.X) && (pre.Y < cur.Y))
                return MouseDirection.Down;

            // Mouse moved left
            if ((pre.X > cur.X) && (pre.Y == cur.Y))
                return MouseDirection.Left;

            // Mouse moved right
            if ((pre.X < cur.X) && (pre.Y == cur.Y))
                return MouseDirection.Right;

            // Mouse moved diagonally up-right
            if ((pre.X < cur.X) && (pre.Y > cur.Y))
                return MouseDirection.TopRight;
            //return MouseDirection.Up;

            // Mouse moved diagonally up-left
            if ((pre.X > cur.X) && (pre.Y > cur.Y))
                return MouseDirection.TopLeft;
            //return MouseDirection.Up;

            // Mouse moved diagonally down-right
            if ((pre.X < cur.X) && (pre.Y < cur.Y))
                return MouseDirection.BottomRight;
            //return MouseDirection.Down;

            // Mouse moved diagonally down-left
            if ((pre.X > cur.X) && (pre.Y < cur.Y))
                return MouseDirection.BottomLeft;
            //return MouseDirection.Down;

            // Mouse didn't move
            return MouseDirection.None;
        }
    }
}
