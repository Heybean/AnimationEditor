using AnimationEditor.Graphics;
using AnimationEditor.Model;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class SpritePropertiesViewModel : ViewModelBase
    {
        private string _spriteName;
        private int? _fps;
        private int? _originX;
        private int? _originY;
        private SpriteHorizontalAlignment? _hAlign;
        private SpriteVerticalAlignment? _vAlign;
        private IList<object> _selectedItems;
        private bool _doNotInvoke;

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
                _fps = UpdateSelectedItems(value) ? value : null;
                OnPropertyChanged();
                MarkFileModified();
            }
        }

        public int? OriginX
        {
            get => GetValue(ref _originX) ? _originX : null;
            set
            {
                _originX = UpdateSelectedItems(value) ? value : null;
                OnPropertyChanged();
                MarkFileModified();

                OnUpdateOriginMarker?.Invoke(this, new OriginUpdatedEventArgs { X = _originX, Y = _originY });

                _hAlign = SpriteHorizontalAlignment.Custom;
                OnPropertyChanged("HAlign");
            }
        }

        public int? OriginY
        {
            get => GetValue(ref _originY) ? _originY : null;
            set
            {
                _originY = UpdateSelectedItems(value) ? value : null;
                OnPropertyChanged();
                MarkFileModified();

                OnUpdateOriginMarker?.Invoke(this, new OriginUpdatedEventArgs { X = _originX, Y = _originY });

                _vAlign = SpriteVerticalAlignment.Custom;
                OnPropertyChanged("VAlign");
            }
        }

        public SpriteHorizontalAlignment? HAlign
        {
            get => GetValue(ref _hAlign) ? _hAlign : null;
            set
            {
                _hAlign = UpdateSelectedItems(value) ? value : null;
                OnPropertyChanged();
                OnPropertyChanged("OriginX");
                OnPropertyChanged("OriginY");
                MarkFileModified();

                if (value != SpriteHorizontalAlignment.Custom)
                    OnUpdateOriginMarker?.Invoke(this, new OriginUpdatedEventArgs { X = _originX, Y = _originY });
            }
        }

        public SpriteVerticalAlignment? VAlign
        {
            get => GetValue(ref _vAlign) ? _vAlign : null;
            set
            {
                _vAlign = UpdateSelectedItems(value) ? value : null;
                OnPropertyChanged();
                OnPropertyChanged("OriginX");
                OnPropertyChanged("OriginY");
                MarkFileModified();

                if (value != SpriteVerticalAlignment.Custom)
                    OnUpdateOriginMarker?.Invoke(this, new OriginUpdatedEventArgs { X = _originX, Y = _originY });
            }
        }

        public delegate void OriginUpdatedEventHandler(object sender, OriginUpdatedEventArgs e);
        public event OriginUpdatedEventHandler OnUpdateOriginMarker;

        public SpritePropertiesViewModel()
        {
            _selectedItems = new List<object>();
        }

        public void Reset()
        {
            SpriteName = "";
            FPS = null;
            OriginX = null;
            OriginY = null;
            HAlign = null;
            VAlign = null;
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

            OnPropertyChanged("FPS");
            OnPropertyChanged("OriginX");
            OnPropertyChanged("OriginY");
            OnPropertyChanged("HAlign");
            OnPropertyChanged("VAlign");
            OnUpdateOriginMarker?.Invoke(this, new OriginUpdatedEventArgs { X = _originX, Y = _originY });
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
