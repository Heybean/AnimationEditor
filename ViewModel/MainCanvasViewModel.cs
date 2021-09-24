using PropertyTools;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class MainCanvasViewModel : Observable
    {
        private Vector2 _originPoint;

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

        public MainCanvasViewModel()
        {
        }

        public void TextureAtlasSelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
