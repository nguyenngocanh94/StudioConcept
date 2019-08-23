using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StudioConcept.MVVM;
using StudioConcept.Tree;

namespace StudioConcept
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public Node Shapes { get; set; }
        public SecondWindow()
        {
            Shapes = new Node(new IfShape(300, 80, Colors.Aqua, "IF"));
            Shapes.ChildNodes.Add(new Node(new Sequence(300, 80, Colors.Brown, "Web")));
            InitializeComponent();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
            DataContext = this;
        }
    }
}
