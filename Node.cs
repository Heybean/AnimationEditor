using PropertyTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace AnimationEditor
{
    public class Node : Observable
    {
        private bool _showChildren;
        public string Name { get; set; }
        public Node Parent { get; }
        public SortableObservableCollection<Node> SubNodes { get; }

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
            SubNodes = new SortableObservableCollection<Node>();
        }

        public void SortSubNodes()
        {
            SubNodes.Sort(node => node.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
