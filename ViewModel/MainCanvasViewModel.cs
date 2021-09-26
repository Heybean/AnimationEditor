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
        private Vector2 _originPoint;
        private Vector2 _outlineSize;
        private Vector2 _canvasSize;
        private ScaleTransform _renderScale;
        private double _originMarkerX;
        private double _originMarkerY;
        private double _originMarkerWidth;
        private double _originMarkerHeight;
        private double _spriteX;
        private double _spriteY;
        private double _spriteWidth;
        private double _spriteHeight;
        private double _outlineThickness;
        private int _zoomIndex;
        private ImageBrush _displaySprite;

        public double OriginMarkerX
        {
            get => _originMarkerX;
            set
            {
                _originMarkerX = value;
                OnPropertyChanged();
            }
        }

        public double OriginMarkerY
        {
            get => _originMarkerY;
            set
            {
                _originMarkerY = value;
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
        public double OutlineThickness
        {
            get => _outlineThickness;
            set
            {
                _outlineThickness = value;
                OnPropertyChanged();
            }
        }

        public double SpriteX
        {
            get => _spriteX;
            set
            {
                _spriteX = value;
                OnPropertyChanged();
            }
        }
        public double SpriteY
        {
            get => _spriteY;
            set
            {
                _spriteY = value;
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
                CenterPositionSprite();
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

        public double SpriteWidth
        {
            get => _spriteWidth;
            set
            {
                _spriteWidth = value;
                OnPropertyChanged();
            }
        }

        public double SpriteHeight
        {
            get => _spriteHeight;
            set
            {
                _spriteHeight = value;
                OnPropertyChanged();
            }
        }

        public ICommand SizeChangedCommand { get; }
        public ICommand MouseWheelCommand { get; }
        public ICommand CanvasLoadedCommand { get; }
        public ICommand OriginMarkerLoadedCommand { get; }

        public MainCanvasViewModel()
        {
            SizeChangedCommand = new RelayCommand(x => SizeChangedExecute(x));
            MouseWheelCommand = new RelayCommand(x => MouseWheelExecute(x));
            CanvasLoadedCommand = new RelayCommand(x => CanvasLoadedExecute(x));
            OriginMarkerLoadedCommand = new RelayCommand(x => OriginMarkerLoadedExecute(x));
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
                    CenterPositionSprite();
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

        public void OnOriginUpdated(object sender, OriginUpdatedEventArgs e)
        {
            if (e.X == null || e.Y == null)
            {
                OriginMarkerX = _canvasSize.X / 2;
                OriginMarkerY = _canvasSize.Y / 2;
                return;
            }

            _originPoint.X = (int)e.X;
            _originPoint.Y = (int)e.Y;

            // Update the origin marker position
            OriginMarkerX = SpriteX + _originPoint.X * _renderScale.ScaleX - _originMarkerWidth / 2;
            OriginMarkerY = SpriteY + _originPoint.Y * _renderScale.ScaleY - _originMarkerHeight / 2;
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

            _canvasSize = new Vector2((float)canvas.ActualWidth, (float)canvas.ActualHeight);
            CenterPositionSprite();
        }

        private void CanvasLoadedExecute(object parameters)
        {
            SizeChangedExecute(parameters);
            OnOriginUpdated(this, new OriginUpdatedEventArgs());
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

        private void OriginMarkerLoadedExecute(object parameters)
        {
            var img = parameters as Image;
            if (img == null)
                return;

            _originMarkerWidth = img.ActualWidth;
            _originMarkerHeight = img.ActualHeight;
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

        private void UpdateOutlineThickness()
        {
            OutlineThickness = 1f / (ZoomIndex + 1);
        }

        private void CenterPositionSprite()
        {
            SpriteX = (_canvasSize.X - _spriteWidth * _renderScale.ScaleX) / 2;
            SpriteY = (_canvasSize.Y - _spriteHeight * _renderScale.ScaleY) / 2;

            OnOriginUpdated(this, new OriginUpdatedEventArgs { X = (int)_originPoint.X, Y = (int)_originPoint.Y });
        }
    }
}
