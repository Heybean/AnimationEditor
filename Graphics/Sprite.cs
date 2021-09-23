using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace AnimationEditor.Graphics
{
    [XmlRoot("Sprite")]
    [XmlInclude(typeof(Sprite))]
    public class Sprite
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("fps")]
        public int FPS { get; set; }

        [XmlIgnore]
        public int RegionIndex { get; set; }

        [XmlIgnore]
        public int OriginX
        {
            get => _originX;
            set
            {
                _originX = value;
                _hAlign = SpriteHorizontalAlignment.Custom;
            }
        }

        [XmlIgnore]
        public int OriginY
        {
            get =>  _originY;
            set
            {
                _originY = value;
                _vAlign = SpriteVerticalAlignment.Custom;
            }
        }

        [XmlIgnore]
        public SpriteHorizontalAlignment HorizontalAlignment
        {
            get => _hAlign;
            set
            {
                _hAlign = value;
                UpdateAlignmentOriginX();
            }
        }

        [XmlIgnore]
        public SpriteVerticalAlignment VerticalAlignment
        {
            get => _vAlign;
            set
            {
                _vAlign = value;
                UpdateAlignmentOriginY();
            }
        }

        [XmlIgnore]
        public List<TextureRegion> Regions { get; private set; }

        [XmlIgnore]
        public ImageBrush CurrentFrame { get; set; }

        private SpriteHorizontalAlignment _hAlign;
        private SpriteVerticalAlignment _vAlign;
        private int _originX;
        private int _originY;
        private double _timeElapsed;

        public Sprite() { }

        public Sprite(string name, List<TextureRegion> regions)
        {
            Name = name;
            Regions = regions;
            FPS = 60;
            HorizontalAlignment = SpriteHorizontalAlignment.Center;
            VerticalAlignment = SpriteVerticalAlignment.Center;
            UpdateAlignmentOriginX();
            UpdateAlignmentOriginY();
            CurrentFrame = Regions[0].ImageBrush;
        }

        public void Update(double time)
        {
            //if (_paused || FPS <= 0)
            //    return;
            if (FPS <= 0)
                return;

            // increment time elapsed
            _timeElapsed += time;

            float timePerFrame = 1000f / FPS;

            // Update sprite to go to the next frame if enough time has passed
            while (_timeElapsed >= timePerFrame)
            {
                RegionIndex = (RegionIndex + 1) % Regions.Count;

                CurrentFrame = Regions[RegionIndex].ImageBrush;

                _timeElapsed -= timePerFrame;
            }

        }

        private void UpdateAlignmentOriginX()
        {
            var region = Regions[0];
            var holdHalign = _hAlign;

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

            _hAlign = holdHalign;
        }

        private void UpdateAlignmentOriginY()
        {
            var region = Regions[0];
            var holdValign = _vAlign;

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

            _vAlign = holdValign;
        }
    }
}
