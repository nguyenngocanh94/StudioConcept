using System;
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
            InteractShape.State = ShapeState.Balance;
            InteractShape = null;
            InteractDirection = null;
        }
    }
    // state of a shape
    public enum ShapeState
    {
        Balance,
        Down,
        Up,
        Middle
    }

}
