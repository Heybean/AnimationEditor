using PropertyTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnimationEditor
{
    public class TreeListBoxNode : Observable
    {
        public string Name { get; set; }
        public TreeListBoxNode Parent { get; }
        public IList<TreeListBoxNode> SubNodes { get; }

        public TreeListBoxNode(TreeListBoxNode parent = null)
        {
            Parent = parent;
            SubNodes = new ObservableCollection<TreeListBoxNode>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
