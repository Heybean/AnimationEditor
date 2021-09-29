using AnimationEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AnimationEditor.ViewModel
{
    public class SpritePreviewViewModel : ViewModelBase
    {
        private Visibility _isVisible;
        private SpriteModel _selectedSprite;
        private ImageBrush _displaySprite;
        private ScaleTransform _renderScale;
        private int _zoomIndex;
        private double _spriteWidth;
        private double _spriteHeight;
        private double _spriteX;
        private double _spriteY;
        private Vector2 _canvasSize;
        private DispatcherTimer _timer;
        private List<DrawSpriteModel> _sprites;

        public Visibility Visible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public List<DrawSpriteModel> Sprites
        {
            get => _sprites;
        }

        public ImageBrush CurrentFrame
        {
            get => _displaySprite;
            set
            {
                _displaySprite = value;
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

        public ICommand ClosingCommand { get; }
        public ICommand SizeChangedCommand { get; }

        public SpritePreviewViewModel()
        {
            ClosingCommand = new RelayCommand(x => ClosingExecute(x));
            SizeChangedCommand = new RelayCommand(x => SizeChangedExecute(x));
            RenderScale = new ScaleTransform(2, 2);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / 60.0);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            _sprites = new List<DrawSpriteModel>();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (_selectedSprite != null)
            {
                int prevIndex = _selectedSprite.RegionIndex;
                _selectedSprite.Update(_timer.Interval.TotalMilliseconds);
                if (_selectedSprite.RegionIndex != prevIndex)
                {
                    CurrentFrame = _selectedSprite.CurrentFrame;
                }
            }
        }

        public void TextureAtlasSelectionChanged(object sender, EventArgs e)
        {
            var selectedItems = (IList<object>)e.Parameters;

            if (selectedItems.Count == 1 && selectedItems[0] is SpriteModel sprite)
            {
                _selectedSprite = sprite;
                CurrentFrame = sprite.CurrentFrame;
                SpriteWidth = sprite.Regions[0].width;
                SpriteHeight = sprite.Regions[0].height;

                CenterPositionSprite();
            }
            else
            {
                _selectedSprite = null;
                CurrentFrame = null;
            }
        }

        private void ClosingExecute(object parameters)
        {
            var args = (CancelEventArgs)parameters;

            args.Cancel = true;
            Visible = Visibility.Hidden;
        }

        private void SizeChangedExecute(object parameters)
        {
            var canvas = (Canvas)parameters;

            _canvasSize.X = (float)canvas.ActualWidth;
            _canvasSize.Y = (float)canvas.ActualHeight;

            CenterPositionSprite();
        }

        private void CenterPositionSprite()
        {
            SpriteX = (_canvasSize.X - _spriteWidth * _renderScale.ScaleX) / 2;
            SpriteY = (_canvasSize.Y - _spriteHeight * _renderScale.ScaleY) / 2;
        }
    }
}
