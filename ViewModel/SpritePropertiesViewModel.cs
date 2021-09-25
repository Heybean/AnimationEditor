using AnimationEditor.Model;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class SpritePropertiesViewModel : NotifyBase
    {
        private string _spriteName;
        private int? _fps;
        private int? _originX;
        private int? _originY;
        private IList<object> _selectedItems;

        public string SpriteName
        {
            get => _spriteName;
            set
            {
                _spriteName = value;
                OnPropertyChanged();
            }
        }

        public int? FPS
        {
            get => GetValue(ref _fps) ? _fps : null;
            set
            {
                if (!UpdateSelectedItems(value))
                    value = null;

                _fps = value;
                OnPropertyChanged();
            }
        }

        public int? OriginX
        {   
            get => _originX;
            set
            {
                //SetValue(ref _originX, value);
            }
        }

        public int? OriginY
        {
            get => _originY;
            set
            {
                //SetValue(ref _originY, value);
            }
        }

        public SpritePropertiesViewModel()
        {
            _selectedItems = new List<object>();
        }

        public void TextureAtlasSelectionChanged(object sender, EventArgs e)
        {
            _selectedItems = (IList<object>)e.Parameters;

            if (_selectedItems.Count == 1)
            {
                var item = _selectedItems[0];
                if (item is SpriteModel sprite)
                    SpriteName = sprite.Name;
                else if (item is Node node)
                    SpriteName = node.Name;
            }
            else if (_selectedItems.Count <= 0)
            {
                SpriteName = "";
            }
            else
            {
                SpriteName = $"Selected Items: [{_selectedItems.Count}]";
            }

            if (!GetValue(ref _fps, "FPS"))
                _fps = null;

            OnPropertyChanged("FPS");
        }

        private bool GetValue<T>(ref T value, [CallerMemberName] string propertyName = "")
        {
            if (IsAllOfType(typeof(SpriteModel)))
            {
                bool gotFirstValue = false;
                var type = typeof(SpriteModel);
                var propertyInfo = type.GetProperty(propertyName);

                foreach (SpriteModel sprite in _selectedItems)
                {
                    if (!gotFirstValue)
                    {
                        value = (T)propertyInfo.GetValue(sprite);
                        gotFirstValue = true;
                    }
                    else if (!EqualityComparer<T>.Default.Equals(value, (T)propertyInfo.GetValue(sprite)))
                        return false;
                }

                return true;
            }

            return false;
        }

        private bool UpdateSelectedItems(object value, [CallerMemberName] string propertyName = "")
        {
            if (!IsAllOfType(typeof(SpriteModel)))
                return false;

            foreach (SpriteModel sprite in _selectedItems)
            {
                sprite.GetType().GetProperty(propertyName).SetValue(sprite, value);
                //_mainWindowViewModel.UnsavedChanges = true;
            }

            return true;
        }

        private bool IsAllOfType(Type type)
        {
            foreach (var item in _selectedItems)
            {
                if (item.GetType() != type)
                    return false;
            }
            return true;
        }
    }
}
