using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationManager.Graphics
{
    public enum SpriteHorizontalAlignment
    {
        Custom,
        Left,
        Center,
        Right
    }

    public enum SpriteVerticalAlignment
    {
        Custom,
        Top,
        Center,
        Bottom
    }

    public class WpfSprite : INotifyPropertyChanged
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
                _hAlign = SpriteHorizontalAlignment.Custom;
                NotifyPropertyChanged();
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
                _vAlign = SpriteVerticalAlignment.Custom;
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public SpriteVerticalAlignment VerticalAlignment
        {
            get
            {
                return _vAlign;
            }

            set
            {
                _vAlign = value;
                SetAlignmentOrigin();
                NotifyPropertyChanged();
            }
        }

        public List<WpfTextureRegion> Regions { get; private set; }

        private int _regionIndex;
        private SpriteHorizontalAlignment _hAlign;
        private SpriteVerticalAlignment _vAlign;
        private int _originX;
        private int _originY;

        public event PropertyChangedEventHandler PropertyChanged;

        public WpfSprite(string name, List<WpfTextureRegion> regions)
        {
            Name = name;
            Regions = regions;
            FPS = 60;
            HorizontalAlignment = SpriteHorizontalAlignment.Center;
            VerticalAlignment = SpriteVerticalAlignment.Center;
            SetAlignmentOrigin();
        }

        public void Update()
        {

        }

        private void SetAlignmentOrigin()
        {
            var region = Regions[0];
            var holdHalign = _hAlign;
            var holdValign = _vAlign;

            switch (HorizontalAlignment)
            {
                case SpriteHorizontalAlignment.Left:
                    OriginX = 0;
                    break;
                case SpriteHorizontalAlignment.Center:
                    OriginX = region.width / 2;
                    break;
                case SpriteHorizontalAlignment.Right:
                    OriginX = region.width;
                    break;
            }

            switch (VerticalAlignment)
            {
                case SpriteVerticalAlignment.Top:
                    OriginY = 0;
                    break;
                case SpriteVerticalAlignment.Center:
                    OriginY = region.height / 2;
                    break;
                case SpriteVerticalAlignment.Bottom:
                    OriginY = region.height;
                    break;
            }

            _hAlign = holdHalign;
            _vAlign = holdValign;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
