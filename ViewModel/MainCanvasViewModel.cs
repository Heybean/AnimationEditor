using AnimationEditor.Model;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace AnimationEditor.ViewModel
{
    public class MainCanvasViewModel : Observable
    {
        private Vector2 _originPoint;
        private Vector2 _spritePosition;
        private Vector2 _outlinePos;
        private Vector2 _outlineSize;
        private float _outlineThickness;

        public float OriginPointX
        {
            get => _originPoint.X;
            set
            {
                SetValue(ref _originPoint.X, value);
            }
        }

        public float OriginPointY
        {
            get => _originPoint.Y;
            set
            {
                SetValue(ref _originPoint.Y, value);
            }
        }

        public float OutlineX
        {
            get => _outlinePos.X;
            set
            {
                SetValue(ref _outlinePos.X, value);
            }
        }
        public float OutlineY
        {
            get => _outlinePos.Y;
            set
            {
                SetValue(ref _outlinePos.Y, value);
            }
        }
        public float OutlineWidth {
            get => _outlineSize.X;
            set
            {
                SetValue(ref _outlineSize.X, value);
            }
        }
        public float OutlineHeight
        {
            get => _outlineSize.Y;
            set
            {
                SetValue(ref _outlineSize.Y, value);
            }
        }
        public float OutlineThickness
        {
            get => _outlineThickness;
            set
            {
                SetValue(ref _outlineThickness, value);
            }
        }

        public float SpriteX
        {
            get => _spritePosition.X;
            set
            {
                SetValue(ref _spritePosition.X, value);
            }
        }
        public float SpriteY
        {
            get => _spritePosition.Y;
            set
            {
                SetValue(ref _spritePosition.Y, value);
            }
        }

        public MainCanvasViewModel()
        {
        }

        public void TextureAtlasSelectionChanged(object sender, EventArgs e)
        {
            var list = e.Parameters as IList<object>;

            if (list.Count == 1)
            {
                if (list[0] is SpriteModel sprite)
                {
                    OutlineWidth = sprite.Regions[0].width;
                    OutlineHeight = sprite.Regions[0].height;
                    OutlineThickness = 1;
                }
                else
                {
                    HideOutlineRect();
                }
            }
            else
            {
                HideOutlineRect();
            }
        }

        private void HideOutlineRect()
        {
            OutlineX = 0;
            OutlineY = 0;
            OutlineWidth = 0;
            OutlineHeight = 0;
            OutlineThickness = 0;
        }
    }
}
