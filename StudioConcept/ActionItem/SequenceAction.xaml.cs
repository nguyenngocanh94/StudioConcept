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

namespace StudioConcept.ActionItem
{
    /// <summary>
    /// Interaction logic for SequenceAction.xaml
    /// </summary>
    public partial class SequenceAction : UserControl
    {
        //public static readonly DependencyProperty ShadowProperty =
        // DependencyProperty.Register("Shadow", typeof(bool), typeof(SequenceAction));

        //public bool Shadow {
        //    get { return (bool)GetValue(ShadowProperty); } 
        // set { SetValue(ShadowProperty, value); }  }
        public SequenceAction()
        {
            InitializeComponent();
        }

    }
}
