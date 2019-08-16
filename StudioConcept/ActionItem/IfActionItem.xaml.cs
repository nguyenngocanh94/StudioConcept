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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudioConcept.MVVM;

namespace StudioConcept.ActionItem
{
    /// <summary>
    /// Interaction logic for IfActionItem.xaml
    /// </summary>
    public partial class IfActionItem
    {

        public static readonly DependencyProperty ShapesProperty =
            DependencyProperty.Register("Shapes", typeof(List<BaseShape>),
                typeof(IfActionItem));

        public List<BaseShape> Shapes
        {
            get => (List<BaseShape>)GetValue(ShapesProperty);
            set => SetValue(ShapesProperty, value);
        }

        public IfActionItem()
        {
            InitializeComponent();
        }
    }
}
