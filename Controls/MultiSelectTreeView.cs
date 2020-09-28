using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AnimationManager.Controls
{
    public class MultiSelectTreeView : TreeView
    {
        public static readonly RoutedEvent ItemsSelectedEvent =
            EventManager.RegisterRoutedEvent("ItemsSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeView));

        public event RoutedEventHandler ItemsSelected
        {
            add { AddHandler(ItemsSelectedEvent, value); }
            remove { RemoveHandler(ItemsSelectedEvent, value); }
        }
    }
}
