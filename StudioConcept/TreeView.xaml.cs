using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using StudioConcept.MVVM;


namespace StudioConcept
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class TreeView : Window
    {
        public List<BaseShape> Shapes { get; set; }
        public TreeView()
        {
            Shapes = new List<BaseShape>();
            BaseShape root = new Sequence(250, 30, Colors.Brown, "Web");

            Shapes.Add(root);
            InitializeComponent();
            DataContext = this;
        }
    }
}
