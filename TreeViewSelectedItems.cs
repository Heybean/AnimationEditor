using AnimationEditor.Graphics;
using AnimationEditor.ViewModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnimationEditor
{
    /// <summary>
    /// A wrapper class for all the selected items in the tree view, mostly done for binding purposes
    /// </summary>
    public class TreeViewSelectedItems : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainWindowViewModel;
        private IList<object> _selectedItems;
        private bool _doNotInvoke;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                if (_selectedItems.Count == 1)
                {
                    var item = _selectedItems[0];
                    if (item is SpriteModel sprite)
                        return sprite.Name;
                    //else if (item is TextureAtlasTreeItem tai)
                    //    return tai.Name;
                }

                return $"Selected Items: [{_selectedItems.Count}]";
            }
        }

        public int? FPS
        {
            get => GetValue<int?>();

            set
            {
                if (!IsAllOfType(typeof(SpriteModel)))
                    return;

                foreach(SpriteModel sprite in _selectedItems)
                {
                    sprite.FPS = value ?? 0;
                    _mainWindowViewModel.UnsavedChanges = true;
                }
                NotifyPropertyChanged();
            }
        }

        public int? OriginX
        {
            get => GetValue<int?>();

            set
            {
                if (!IsAllOfType(typeof(SpriteModel)))
                {
                    return;
                }

                if (_selectedItems.Count > 0)
                    _mainWindowViewModel.UnsavedChanges = true;

                foreach (SpriteModel sprite in _selectedItems)
                {
                    sprite.OriginX = value ?? 0;
                    sprite.HorizontalAlignment = SpriteHorizontalAlignment.Custom;
                }
                if (!_doNotInvoke)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HorizontalAlignment"));
                NotifyPropertyChanged();
            }
        }

        public int? OriginY
        {
            get => GetValue<int?>();

            set
            {
                if (!IsAllOfType(typeof(SpriteModel)))
                {
                    return;
                }

                if (_selectedItems.Count > 0)
                    _mainWindowViewModel.UnsavedChanges = true;

                foreach (SpriteModel sprite in _selectedItems)
                {
                    sprite.OriginY = value ?? 0;
                    sprite.VerticalAlignment = SpriteVerticalAlignment.Custom;
                }
                if (!_doNotInvoke)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VerticalAlignment"));
                NotifyPropertyChanged();
            }
        }

        public SpriteHorizontalAlignment? HorizontalAlignment
        {
            get => GetValue<SpriteHorizontalAlignment?>();
            set
            {
                if (!IsAllOfType(typeof(SpriteModel)))
                {
                    return;
                }

                if (_selectedItems.Count > 0)
                    _mainWindowViewModel.UnsavedChanges = true;

                foreach (SpriteModel sprite in _selectedItems)
                {
                    sprite.HorizontalAlignment = value ?? SpriteHorizontalAlignment.Center;
                }

                NotifyPropertyChanged();
                if (value != SpriteHorizontalAlignment.Custom)
                    NotifyPropertyChanged("OriginX");
            }
        }

        public SpriteVerticalAlignment? VerticalAlignment
        {
            get => GetValue<SpriteVerticalAlignment?>();
            set
            {
                if (!IsAllOfType(typeof(SpriteModel)))
                {
                    return;
                }

                if (_selectedItems.Count > 0)
                    _mainWindowViewModel.UnsavedChanges = true;

                foreach (SpriteModel sprite in _selectedItems)
                {
                    sprite.VerticalAlignment = value ?? SpriteVerticalAlignment.Center;
                }

                NotifyPropertyChanged();
                if (value != SpriteVerticalAlignment.Custom)
                    NotifyPropertyChanged("OriginY");
            }
        }

        public TreeViewSelectedItems(IList<object> selectedItems, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _selectedItems = selectedItems;
            _doNotInvoke = false;
        }

        /// <summary>
        /// Checks to see if all selected items are of the given type
        /// </summary>
        private bool IsAllOfType(Type type)
        {
            foreach(var item in _selectedItems)
            {
                if (item.GetType() != type)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieve value from all the given selected items. If all values are the same, returns that value. If not, returns null.
        /// </summary>
        private T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            if (IsAllOfType(typeof(SpriteModel)))
            {
                T value = default(T);
                var type = typeof(SpriteModel);
                var propertyInfo = type.GetProperty(propertyName);

                foreach (SpriteModel sprite in _selectedItems)
                {
                    if (value == null)
                        value = (T)propertyInfo.GetValue(sprite);
                    else if (!EqualityComparer<T>.Default.Equals(value, (T)propertyInfo.GetValue(sprite)))
                        return default(T);
                }
                return value;
            }

            return default(T);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
