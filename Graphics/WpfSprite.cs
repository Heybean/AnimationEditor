using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationManager.Graphics
{
    public class WpfSprite
    {
        public string Name { get; private set; }
        public int FPS { get; set; }
        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public List<TextureAtlasData.Region> Regions { get; private set; }

        public WpfSprite(string name, List<TextureAtlasData.Region> regions)
        {
            Name = name;
            Regions = regions;
        }
    }
}
