using System.Windows;
using System.Windows.Media;

namespace StudioConcept.MVVM
{
    public class Sequence : BaseShape
    {
        private double width;
        private double height;
        public double Width => width;
        public double Height => height;

        public string Draw()
        {
            return $"m 0,4 A 4,4 0 0,1 4,0 H 12 c 2,0 3,1 4,2 l 4,4 c 1,1 2,2 4,2 h 12 c 2,0 3,-1 4,-2 l 4,-4 c 1,-1 2,-2 4,-2 H {width} a 4,4 0 0,1 4,4 v {height}  a 4,4 0 0,1 -4,4 H 48   c -2,0 -3,1 -4,2 l -4,4 c -1,1 -2,2 -4,2 h -12 c -2,0 -3,-1 -4,-2 l -4,-4 c -1,-1 -2,-2 -4,-2 H 4 a 4,4 0 0,1 -4,-4 z";
        }

        public Color Color { get; set; }
        public Color TextColor { get; set; }
        private string data;
        public string Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        public double FontSize { get; set; }

        private string text;
        public string Text {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public Thickness MarginText { get; set; }

        public Sequence(double width, double height, Color color, string text)
        {
            this.width = width;
            this.height = height;
            Color = color;
            this.text = text;
            FontSize = 15;
            data = Draw();
        }

    }
}
