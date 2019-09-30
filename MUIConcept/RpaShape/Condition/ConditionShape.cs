using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RpaShape.Condition
{
    /// <summary>
    /// this shape is rhombus
    /// </summary>
    public class ConditionShape : Shape
    {
        protected string template = "m 0,0 m {Radius},0 H {Width} l {Radius} {Radius} l -{Radius} {Radius} H {Radius} l -{Radius} -{Radius} l {Radius} -{Radius} z";
        protected override Geometry DefiningGeometry => throw new NotImplementedException();
    }
}
