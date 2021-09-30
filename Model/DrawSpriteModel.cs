using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.Model
{
    /// <summary>
    /// Model to represent the individually rendered sprites on a canvas
    /// </summary>
    public class DrawSpriteModel : NotifyBase
    {
        private double _left;
        private double _top;
        private double _width;
        private double _height;
        private SpriteModel _sprite;

        public double Left
        {
            get => _left;
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }

        public double Top
        {
            get => _top;
            set
            {
                _top = value;
                OnPropertyChanged();
            }
        }

        public SpriteModel Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                Width = Sprite.Regions[0].width;
                Height = Sprite.Regions[0].height;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

    }
}
