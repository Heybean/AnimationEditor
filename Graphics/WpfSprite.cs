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
        public int OriginX
        {
            get
            {
                return _originX;
            }
            set
            {
                _originX = value;
                _hAlign = SpriteHorizontalAlignment.None;
            }
        }

        public int OriginY
        {
            get
            {
                return _originY;
            }
            set
            {
                _originY = value;
                _vAlign = SpriteVerticalAlignment.None;
            }
        }

        public SpriteHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _hAlign;
            }
            set
            {
                _hAlign = value;
                SetAlignmentOrigin();
            }
        }

        public SpriteVerticalAlignment VerticalAligment
        {
            get
            {
                return _vAlign;
            }

            set
            {
                _vAlign = value;
                SetAlignmentOrigin();
            }
        }

        public List<WpfTextureRegion> Regions { get; private set; }

        private int _regionIndex;
        private SpriteHorizontalAlignment _hAlign;
        private SpriteVerticalAlignment _vAlign;
        private int _originX;
        private int _originY;

        public WpfSprite(string name, List<WpfTextureRegion> regions)
        {
            Name = name;
            Regions = regions;
            FPS = 60;
            HorizontalAlignment = SpriteHorizontalAlignment.Center;
            VerticalAligment = SpriteVerticalAlignment.Center;
            SetAlignmentOrigin();
        }

        public void Update()
        {

        }

        private void SetAlignmentOrigin()
        {
            var region = Regions[0];
            switch (HorizontalAlignment)
            {
                case SpriteHorizontalAlignment.Left:
                    _originX = 0;
                    break;
                case SpriteHorizontalAlignment.Center:
                    _originX = region.width / 2;
                    break;
                case SpriteHorizontalAlignment.Right:
                    _originX = region.width;
                    break;
            }

            switch (VerticalAligment)
            {
                case SpriteVerticalAlignment.Top:
                    _originY = 0;
                    break;
                case SpriteVerticalAlignment.Center:
                    _originY = region.height / 2;
                    break;
                case SpriteVerticalAlignment.Bottom:
                    _originY = region.height;
                    break;
            }
        }
    }
}
