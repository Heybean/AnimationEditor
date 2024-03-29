﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AnimationEditor.Controls
{
    public class MultiSelectTreeView : TreeView
    {
        public static readonly RoutedEvent ItemsSelectedEvent =
            EventManager.RegisterRoutedEvent("OnItemsSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeView));

        public event RoutedEventHandler OnItemsSelected
        {
            add { AddHandler(ItemsSelectedEvent, value); }
            remove { RemoveHandler(ItemsSelectedEvent, value); }
        }
    }
}
