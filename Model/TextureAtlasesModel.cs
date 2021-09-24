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
    /// Model for the texture atlases data. All file data can be written and stored from this.
    /// </summary>
    public class TextureAtlasesModel
    {
        private Dictionary<string, TextureAtlasModel> _textureAtlases;

        public IReadOnlyDictionary<string, TextureAtlasModel> TextureAtlases { get => _textureAtlases; }

        public TextureAtlasesModel()
        {
            _textureAtlases = new Dictionary<string, TextureAtlasModel>();
        }

        public TextureAtlasModel AddTextureAtlas(string file)
        {
            var atlasName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (_textureAtlases.ContainsKey(atlasName))
            {
                return null;
            }

            // Load the atlas
            var atlas = new TextureAtlasModel(file);
            _textureAtlases.Add(atlasName, atlas);

            return atlas;
        }

        public bool RemoveTextureAtlas(string atlasName)
        {
            return _textureAtlases.Remove(atlasName);
        }
    }
}
