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
                OnPropertyChanged();
            }
        }
    }
}
