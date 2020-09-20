using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationManager
{
    public class AnimationsData
    {
        public TextureAtlas TextureAtlas
        {
            get; private set;
        }

        public Dictionary<string, Sprite> Sprites
        {
            get; private set;
        }

        public AnimationsData()
        {
            Sprites = new Dictionary<string, Sprite>();
        }
    }
}
