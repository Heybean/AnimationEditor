using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor
{
    public static class Utils
    {
        public static Texture2D LoadTexture2D(GraphicsDevice graphicsDevice, string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            var texture = Texture2D.FromStream(graphicsDevice, fs);
            fs.Dispose();
            return texture;
        }
    }
}
