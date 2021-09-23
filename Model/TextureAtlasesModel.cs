using AnimationEditor.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;

namespace AnimationEditor.Model
{
    /// <summary>
    /// Model for the texture atlases view (left side of window)
    /// </summary>
    public class TextureAtlasesModel
    {
        private Dictionary<string, WpfTextureAtlas> _textureAtlases;

        public IReadOnlyDictionary<string, WpfTextureAtlas> TextureAtlases { get => _textureAtlases; }

        public TextureAtlasesModel()
        {
            _textureAtlases = new Dictionary<string, WpfTextureAtlas>();
        }

        public bool AddTextureAtlas(string file)
        {
            var atlasName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (_textureAtlases.ContainsKey(atlasName))
            {
                return false;
            }

            // Load the atlas
            var atlas = new WpfTextureAtlas(file);
            _textureAtlases.Add(atlasName, atlas);

            return true;
        }

        public bool RemoveTextureAtlas(string atlasName)
        {
            return _textureAtlases.Remove(atlasName);
        }
    }
}
