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
        private string _name;

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
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
    }
}
