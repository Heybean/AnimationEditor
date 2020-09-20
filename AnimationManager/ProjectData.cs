using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
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
        public Dictionary<string, AnimationsData> AnimationAtlases { get; private set; }

        public ProjectData()
        {
            AnimationAtlases = new Dictionary<string, AnimationsData>();
        }
    }
}
