using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using StudioConcept.Observer;

namespace StudioConcept.MVVM
{
    public interface IBranch
    {

    }
    public class IfShape : BaseShape, IBranch
    {
        public double MiddleUpperY => Y + 20 + 8;
        public double MiddleLowerY => Y + MiddleSpace + 20 + 8;
        public override double OuterUpperY => Y - 10 - 4;
        public override double OuterLowerY => InnerLowerY + 10 + 4;
        public override double VirtualOuterLowerY => VirtualInnerLowerY + 10 + 4; 
        public override double VirtualInnerLowerY => Y + Height;

        //don't ask me why
        public override double Height => _middleSpace + 40 + 16;

        public sealed override string Draw()
        {
            string aboveTemplate = $"m 0,4 A 4,4 0 0,1 4,0 H 12 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {Width} a 4,4 0 0,1 4,4 v 20  a 4,4 0 0,1 -4,4 H 64 c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 h -8  a 4,4 0 0,0 -4,4 ";
            string middleSpaceTemplate = $"v {_middleSpace} a 4,4 0 0,0 4,4 ";
            string belowTemplate = $"h 8 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {Width} H {Width} a 4,4 0 0,1 4,4 v 20  a 4,4 0 0,1 -4,4 H 48   c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 H 4 a 4,4 0 0,1 -4,-4 z";

            return aboveTemplate + middleSpaceTemplate + belowTemplate;
        }
        public Color TextColor { get; set; }
        
        public double FontSize { get; set; }

        public Thickness MarginText { get; set; }

        public IfShape(double width, double middleSpace, Color color, string text)
        {
            Width = width;
            _middleSpace = middleSpace;
            Color = color;
            Text = text;
            Data = Draw();
        }

        public IfShape()
        {
            
        }

        public override void UpdateMiddleSpace()
        {
            if (State == Observer.State.ShapeState.Expand)
            {
                State = Observer.State.ShapeState.Balance;
            }
            if (ChildrenNode.Count==0)
            {
                MiddleSpace = 20;
                Parent?.UpdateMiddleSpace();
                return;
            }
            var head = ChildrenNode.FirstOrDefault(i => i.IsHead());
            if (head != null) MiddleSpace = head.VirtualHeight+ 8.8*head.Count-8.8;
            // ReSharper disable once PossibleInvalidOperationException
        }

        public override void Expand(double delta)
        {
            if (State==Observer.State.ShapeState.Balance)
            {
                MiddleSpace += delta;
                State = Observer.State.ShapeState.Expand;
            }
        }

        public override void ArrangeChildrenPosition()
        {
            var head = ChildrenNode.FirstOrDefault(i => i.IsHead());
            int index = 0;
            double step = 0;
            while (head != null)
            {
                head.X = X + 16;
                head.Y = MiddleUpperY + step;
                step += head.Height + 8.8;
                head = head.Next;
            }
        }

    }
}
