using StudioConcept.MVVM;

namespace StudioConcept.Observer
{
    // state
    public class State
    {
        public ShapeState? InteractDirection;
        public BaseShape InteractShape;

        public bool Equal(State other)
        {
            return InteractDirection == other.InteractDirection && InteractShape == other.InteractShape;
        }

        public bool Stateless()
        {
            return InteractShape == null;
        }

        public void Clear()
        {
            if (InteractShape!=null)
            {
                InteractShape.State = ShapeState.Balance;
            }

            InteractShape = null;
            InteractDirection = null;
        }

        public static void Copy(State from, ref State to)
        {
            if (!from.Stateless() && from.InteractDirection != ShapeState.Middle)
            {
                to.InteractShape = from.InteractShape;
                to.InteractDirection = from.InteractDirection;
            }
        }

        public override string ToString()
        {
            return InteractShape?.Text + "=> State " + InteractShape?.State + " => InteractState Direction " +
                   (InteractDirection == ShapeState.Down ? "DOWN" :
                       InteractDirection == null ? "NULL" :
                       InteractDirection == ShapeState.Middle ? "MIDDLE" : "UP");
        }

        // state of a shape
        public enum ShapeState
        {
            Balance,
            Down,
            Changed,
            Middle,
            Expand
        }
    }
}
