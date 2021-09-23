using PropertyTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnimationEditor
{
    public class Node : Observable
    {
        private bool _showChildren;

        public string Name { get; set; }
        public Node Parent { get; }
        public IList<Node> SubNodes { get; }

        public bool ShowChildren
        {
            get => _showChildren;
            set
            {
                SetValue(ref _showChildren, value);
            }
        }

        public Node(Node parent = null)
        {
            Parent = parent;
            SubNodes = new ObservableCollection<Node>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
