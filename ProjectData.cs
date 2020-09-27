using AnimationManager.Graphics;
using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationManager
{
    /// <summary>
    /// Holds information on current working data
    /// </summary>
    public class ProjectData
    {
        public string Name { get; set; }
        public ObservableCollection<WpfTextureAtlas> TextureAtlases { get; } = new ObservableCollection<WpfTextureAtlas>();
        public HashSet<string> RegisteredTextureAtlases { get; } = new HashSet<string>();

        public ProjectData()
        {
            Name = "Untitled";
        }
    }
}
