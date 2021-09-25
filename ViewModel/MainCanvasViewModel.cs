using AnimationEditor.Model;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AnimationEditor.ViewModel
{
    public class MainCanvasViewModel : ViewModelBase
    {
        private Vector2 _spriteOrigSize;
        private Vector2 _spriteSize;
        private Vector2 _originPoint;
        private Vector2 _spritePosition;
        private Vector2 _outlineSize;
        private ScaleTransform _renderScale;
        private float _outlineThickness;
        private int _zoomIndex;
        private ImageBrush _displaySprite;

        public float OriginPointX
        {
            get => _originPoint.X;
            set
            {
                _originPoint.X = value;
                OnPropertyChanged();
            }
        }

        public float OriginPointY
        {
            get => _originPoint.Y;
            set
            {
                _originPoint.Y = value;
                OnPropertyChanged();
            }
        }

        public float OutlineWidth
        {
            get => _outlineSize.X;
            set
            {
                _outlineSize.X = value;
                OnPropertyChanged();
            }
        }
        public float OutlineHeight
        {
            get => _outlineSize.Y;
            set
            {
                _outlineSize.Y = value;
                OnPropertyChanged();
            }
        }
        public float OutlineThickness
        {
            get => _outlineThickness;
            set
            {
                _outlineThickness = value;
                OnPropertyChanged();
            }
        }

        public float SpriteX
        {
            get => _spritePosition.X;
            set
            {
                _spritePosition.X = value;
                OnPropertyChanged();
            }
        }
        public float SpriteY
        {
            get => _spritePosition.Y;
            set
            {
                _spritePosition.Y = value;
                OnPropertyChanged();
            }
        }

        public ScaleTransform RenderScale
        {
            get => _renderScale;
            set
            {
                _renderScale = value;
                OnPropertyChanged();
                UpdateOutlineThickness();
            }
        }

        public int ZoomIndex
        {
            get => _zoomIndex;
            set
            {
                _zoomIndex = value;
                OnPropertyChanged();

                _renderScale.ScaleX = value + 1;
                _renderScale.ScaleY = value + 1;
                RenderScale = _renderScale;
                RefreshSprite();
            }
        }

        public ImageBrush DisplaySprite
        {
            get => _displaySprite;
            set
            {
                _displaySprite = value;
                OnPropertyChanged();
            }
        }

        public float SpriteWidth
        {
            get => _spriteSize.X;
            set
            {
                _spriteSize.X = value;
                OnPropertyChanged();
            }
        }

        public float SpriteHeight
        {
            get => _spriteSize.Y;
            set
            {
                _spriteSize.Y = value;
                OnPropertyChanged();
            }
        }


        public ICommand SizeChangedCommand { get; }
        public ICommand MouseWheelCommand { get; }

        public MainCanvasViewModel()
        {
            SizeChangedCommand = new RelayCommand(x => SizeChangedExecute(x));
            MouseWheelCommand = new RelayCommand(x => MouseWheelExecute(x));
            _renderScale = new ScaleTransform();
            ZoomIndex = 2;
        }

        public void TextureAtlasSelectionChanged(object sender, EventArgs e)
        {
            var list = e.Parameters as IList<object>;

            if (list.Count == 1)
            {
                if (list[0] is SpriteModel sprite)
                {
                    LoadSprite(sprite);
                }
                else
                {
                    HideOutlineRect();
                    DisplaySprite = null;
                }
            }
            else
            {
                HideOutlineRect();
                DisplaySprite = null;
            }
        }

        private void HideOutlineRect()
        {
            OutlineWidth = 0;
            OutlineHeight = 0;
            OutlineThickness = 0;
        }

        private void SizeChangedExecute(object parameters)
        {
            var canvas = parameters as Canvas;

            if (canvas == null)
                return;

            RefreshDisplay(canvas.ActualWidth, canvas.ActualHeight);
        }
        private void MouseWheelExecute(object parameters)
        {
            var e = (MouseWheelEventArgs)parameters;

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                int index = ZoomIndex;
                const int MaxCount = 10;

                if (e.Delta > 0)
                {
                    ZoomIndex = Math.Min(index + 1, MaxCount - 1);
                }
                else if (e.Delta < 0)
                {
                    ZoomIndex = Math.Max(index - 1, 0);
                }
            }
        }

        private void RefreshDisplay(double w, double h)
        {
            /*_renderScale.CenterX = OutlineWidth / 2;
            _renderScale.CenterY = OutlineHeight / 2;

            //_spriteDisplay.RenderTransform = _mainRenderScale;

            SpriteX = (int)(w - OutlineWidth) / 2;
            SpriteY = (int)(h - OutlineHeight) / 2;

            OutlineX = (int)(w - OutlineWidth * RenderScale.ScaleX) / 2;
            OutlineY = (int)(h - OutlineHeight * RenderScale.ScaleY) / 2;*/
        }

        private void LoadSprite(SpriteModel sprite)
        {
            OutlineWidth = sprite.Regions[0].width;
            OutlineHeight = sprite.Regions[0].height;
            UpdateOutlineThickness();

            _spriteOrigSize = new Vector2(sprite.Regions[0].width, sprite.Regions[0].height);
            DisplaySprite = sprite.Regions[0].ImageBrush;
            SpriteWidth = _spriteOrigSize.X;
            SpriteHeight = _spriteOrigSize.Y;
            //RefreshSprite();
        }

        private void RefreshSprite()
        {
            //SpriteWidth = (float)(_spriteOrigSize.X * _renderScale.ScaleX);
            //SpriteHeight = (float)(_spriteOrigSize.Y * _renderScale.ScaleY);
        }

        private void UpdateOutlineThickness()
        {
            OutlineThickness = 1f / (ZoomIndex + 1);
        }

        /*private UpdateMainRender()
        {
            int x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width) / 2;
            int y = (int)(_mainRender.ActualHeight - _spriteDisplay.Height) / 2;

            _mainRenderScale.CenterX = _spriteDisplay.Width / 2;
            _mainRenderScale.CenterY = _spriteDisplay.Height / 2;

            _spriteDisplay.RenderTransform = _mainRenderScale;

            Canvas.SetLeft(_spriteDisplay, x);
            Canvas.SetTop(_spriteDisplay, y);

            // Render the outline
            _spriteOutline.Width = _spriteDisplay.Width * _mainRenderScale.ScaleX + 2;
            _spriteOutline.Height = _spriteDisplay.Height * _mainRenderScale.ScaleY + 2;

            x = (int)(_mainRender.ActualWidth - _spriteDisplay.Width * _mainRenderScale.ScaleX) / 2;
            y = (int)(_mainRender.ActualHeight - _spriteDisplay.Height * _mainRenderScale.ScaleY) / 2;

            Canvas.SetLeft(_spriteOutline, x - 1);
            Canvas.SetTop(_spriteOutline, y - 1); */
    }
}
