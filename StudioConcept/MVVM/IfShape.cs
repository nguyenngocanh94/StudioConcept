using System.Windows;
using System.Windows.Media;

namespace StudioConcept.MVVM
{
    public interface IBranch
    {

    }
    public class IfShape : BaseShape, IBranch
    {
        private double _middleSpace;
        public double MiddleSpace { get=>_middleSpace;
            set { _middleSpace = value; Data=Draw();
                
            }
        }
        public double middleUpperY => Y + 20 + 8;
        public double middleLowerY => Y + MiddleSpace + 20 + 8;
        public override double OuterUpperY => Y - 20 - 8;
        public override double OuterLowwerY => InnerLowerY + 20 + 8;
        public override double InnerLowerY => Y + Height;
        //dont ask me why
        public override double Height => _middleSpace + 40 + 16;
        public override string Draw()
        {
            string aboveTemplate = $"m 0,4 A 4,4 0 0,1 4,0 H 12 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {Width} a 4,4 0 0,1 4,4 v 20  a 4,4 0 0,1 -4,4 H 64 c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 h -8  a 4,4 0 0,0 -4,4 ";
            string middleSpaceTemplate = $"v {_middleSpace} a 4,4 0 0,0 4,4 ";
            string belowTemplate = $"h 8 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {Width} H {Width} a 4,4 0 0,1 4,4 v 20  a 4,4 0 0,1 -4,4 H 48   c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 H 4 a 4,4 0 0,1 -4,-4 z";

            return aboveTemplate + middleSpaceTemplate + belowTemplate;
        }

        public Color Color { get; set; }
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

    }
}
