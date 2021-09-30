using AnimationEditor.Model;
using PropertyTools.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IList<object> _selectedItems;
        private List<DrawSpriteModel> _layerSprites;
        private Visibility _isVisible;
        private ScaleTransform _renderScale;
        private int _zoomIndex;
        private Vector2 _canvasSize;
        private DispatcherTimer _timer;
        private bool _play;

        public Visibility Visible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DrawSpriteModel> Sprites
        {
            get;
        } = new ObservableCollection<DrawSpriteModel>();

        public ScaleTransform RenderScale
        {
            get => _renderScale;
            set
            {
                _renderScale = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlaying
        {
            get => _play;
            set
            {
                _play = value;
                OnPropertyChanged();
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
            }
        }

        public ICommand ClosingCommand { get; }
        public ICommand SizeChangedCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand MouseWheelCommand { get; }

        public SpritePreviewViewModel()
        {
            ClosingCommand = new RelayCommand(x => ClosingExecute(x));
            SizeChangedCommand = new RelayCommand(x => SizeChangedExecute(x));
            MouseWheelCommand = new RelayCommand(x => MouseWheelExecute(x));
            PlayCommand = new DelegateCommand(PlayExecute);
            RenderScale = new ScaleTransform();
            ZoomIndex = 1;

            _layerSprites = new List<DrawSpriteModel>();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / 60.0);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            IsPlaying = true;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            /*if (_selectedSprite != null)
            {
                int prevIndex = _selectedSprite.RegionIndex;
                if (IsPlaying)
                    _selectedSprite.Update(_timer.Interval.TotalMilliseconds);
                if (_selectedSprite.RegionIndex != prevIndex)
                {
                    CurrentFrame = _selectedSprite.CurrentFrame;
                }
            }*/
        }

        public void TextureAtlasesVM_SelectionChanged(object sender, EventArgs e)
        {
            _selectedItems = (IList<object>)e.Parameters;

            RefreshCollection();
        }

        public void SpriteControls_LayersUpdated(object sender, EventArgs e)
        {
            _layerSprites.Clear();
  
            var list = (IEnumerable<object>)e.Parameters;

            int zind = 0;
            foreach(SpriteModel item in list)
            {
                var spr = new DrawSpriteModel
                {
                    Sprite = item,
                    ZIndex = zind++
                };

                _layerSprites.Add(spr);
            }

            RefreshCollection();
        }

        private void RefreshCollection()
        {
            Sprites.Clear();

            int zind = 0;
            foreach (var item in _selectedItems)
            {
                if (item is SpriteModel sprite)
                {
                    Sprites.Add(new DrawSpriteModel
                    {
                        Sprite = sprite,
                        ZIndex = zind++
                    });
                }
            }

            foreach (var item in _layerSprites)
            {
                item.ZIndex = zind++;
                Sprites.Add(item);
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
        }

        private void MouseWheelExecute(object parameters)
        {
            var e = (MouseWheelEventArgs)parameters;

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                int index = ZoomIndex;
                const int MaxCount = 5;

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

        private void PlayExecute()
        {
            IsPlaying = !IsPlaying;
        }
    }
}
