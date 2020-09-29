using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationManager.Graphics
{
    public enum SpriteHorizontalAlignment
    {
        None,
        Left,
        Center,
        Right
    }

    public enum SpriteVerticalAlignment
    {
        None,
        Top,
        Center,
        Bottom
    }

    public class WpfSprite
    {
        public string Name { get; private set; }
        public int FPS { get; set; }
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public SpriteHorizontalAlignment HorizontalAlignment { get; set; }
        public SpriteVerticalAlignment VerticalAligment { get; set; }

        public List<WpfTextureRegion> Regions { get; private set; }
        private int _regionIndex;

        public WpfSprite(string name, List<WpfTextureRegion> regions)
        {
            Name = name;
            Regions = regions;
            FPS = 60;
            OriginX = 0;
            OriginY = 0;
            HorizontalAlignment = SpriteHorizontalAlignment.Center;
            VerticalAligment = SpriteVerticalAlignment.Center;
        }

        public void Update()
        {

        }
    }
}
