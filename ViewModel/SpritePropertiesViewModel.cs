using AnimationEditor.Model;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class SpritePropertiesViewModel : Observable
    {
        private string _spriteName;
        private IList<object> _selectedItems;

        public string SpriteName
        {
            get => _spriteName;
            set => SetValue(ref _spriteName, value);
        }

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

        public int FPS
        {
            get;
            set;
        }

        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public SpritePropertiesViewModel()
        {

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
            else
            {
                SpriteName = $"Selected Items: [{_selectedItems.Count}]";
            }
        }
    }
}
