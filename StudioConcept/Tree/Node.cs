using StudioConcept.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioConcept.Tree
{
    public class Node
    {
        List<Node> childNodes;

        public BaseShape Shape { get; set; }
        public List<Node> ChildNodes
        {
            get
            {
                if (childNodes == null)
                    childNodes = new List<Node>();
                return childNodes;
            }
        }

        public Node(BaseShape shape)
        {
            Shape = shape;
        }
    }
}
