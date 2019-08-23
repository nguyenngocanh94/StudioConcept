using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using StudioConcept.Tree;

namespace StudioConcept.MVVM
{
    public class Sequence : BaseShape
    {
       
       
        public override string Draw()
        {
            return $"m 0,4 A 4,4 0 0,1 4,0 H 12 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {Width} a 4,4 0 0,1 4,4 v {Height}  a 4,4 0 0,1 -4,4 H 48   c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 H 4 a 4,4 0 0,1 -4,-4 z";
        }

        public Color Color { get; set; }
        public Color TextColor { get; set; }
        

        public double FontSize { get; set; }

       
        public Thickness MarginText { get; set; }

        public Sequence(double width, double height, Color color, string text)
        {
            Width = width;
            Height = height;
            Color = color;
            Text = text;
            FontSize = 15;
            Data = Draw();
        }

        public Sequence()
        {
            
        }
    }
}
