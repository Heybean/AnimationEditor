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
    public class Node : NotifyBase
    {
        private bool _showChildren;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public Node Parent { get; }
        public SortableObservableCollection<Node> SubNodes { get; }

        public bool ShowChildren
        {
            get => _showChildren;
            set
            {
                _showChildren = value;
                OnPropertyChanged();
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
