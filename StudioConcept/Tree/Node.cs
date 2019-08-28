using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using StudioConcept.MVVM;

namespace StudioConcept.Tree
{
    public interface INode
    {
        ObservableCollection<BaseShape> ChildrenNode { get; }
        /// <summary>
        /// Next pointer for double linked list.
        /// </summary>
        BaseShape Next { get; set; }
        /// <summary>
        /// Prev pointer for double linked list.
        /// </summary>
        BaseShape Prev { get; set; }

        bool IsTail();
        bool IsHead();

        bool IsAlone();

        BaseShape GetHead();

        BaseShape GetTail();
        bool Contain(BaseShape other);
    }
}
