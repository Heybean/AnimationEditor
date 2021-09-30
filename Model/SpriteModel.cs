﻿using AnimationEditor.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace AnimationEditor.Model
{

    [XmlRoot("Sprite")]
    [XmlInclude(typeof(SpriteModel))]
    public class SpriteModel : Node
    {
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
                OnPropertyChanged();
                /*if (!_doNotInvoke)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HorizontalAlignment"));
                NotifyPropertyChanged();*/
                
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
                OnPropertyChanged();
                /*if (!_doNotInvoke)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VerticalAlignment"));
                NotifyPropertyChanged();*/
            }
        }

        [XmlIgnore]
        public SpriteHorizontalAlignment HAlign
        {
            get => _hAlign;
            set
            {
                _hAlign = value;
                _doNotInvoke = true;
                UpdateAlignmentOriginX();
                _doNotInvoke = false;
                //NotifyPropertyChanged();
            }
        }

        [XmlIgnore]
        public SpriteVerticalAlignment VAlign
        {
            get => _vAlign;
            set
            {
                _vAlign = value;
                _doNotInvoke = true;
                UpdateAlignmentOriginY();
                _doNotInvoke = false;
                //NotifyPropertyChanged();
            }
        }

        [XmlIgnore]
        public List<WpfTextureRegion> Regions { get; private set; }

        [XmlIgnore]
        public ImageBrush CurrentFrame
        {
            get => _currentFrame;
            set
            {
                if (_currentFrame != value)
                    OnPropertyChanged();
                _currentFrame = value;
            }
        }


        private SpriteHorizontalAlignment _hAlign;
        private SpriteVerticalAlignment _vAlign;
        private ImageBrush _currentFrame;
        private int _originX;
        private int _originY;
        private double _timeElapsed;
        private bool _doNotInvoke;

        public SpriteModel() { }

        public SpriteModel(string name, List<WpfTextureRegion> regions)
        {
            Name = name;
            Regions = regions;
            FPS = 60;
            HAlign = SpriteHorizontalAlignment.Center;
            VAlign = SpriteVerticalAlignment.Center;
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

            switch (HAlign)
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

            switch (VAlign)
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

        /*private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}
