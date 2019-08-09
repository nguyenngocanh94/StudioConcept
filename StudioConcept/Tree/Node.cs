using System.Collections.Generic;
using StudioConcept.MVVM;

namespace StudioConcept.Tree
{
    public interface INode
    {
        List<BaseShape> ChildrenNode { get; }
    }
}
