using AnimationEditor.Model;
using AnimationEditor.View;
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
        private HashSet<string> _recordedSprites;
        private List<DrawSpriteModel> _layerSprites;
        private Visibility _visiblity;
        private ScaleTransform _renderScale;
        private int _zoomIndex;
        private Vector2 _canvasSize;
        private DispatcherTimer _timer;
        private bool _play;

        public Visibility Visibility
        {
            get => _visiblity;
            set
            {
                _visiblity = value;
                AppSettings.Instance.PreviewVisibility = value;
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
                AppSettings.Instance.PreviewZoomIndex = value;
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
        public ICommand ResetFramesCommand { get; }
        public ICommand FrameLeftCommand { get; }
        public ICommand FrameRightCommand { get; }
        public ICommand ToggleVisiblityCommand { get; }

        public SpritePreviewViewModel()
        {
            ClosingCommand = new RelayCommand(x => ClosingExecute(x));
            SizeChangedCommand = new RelayCommand(x => SizeChangedExecute(x));
            MouseWheelCommand = new RelayCommand(x => MouseWheelExecute(x));
            PlayCommand = new DelegateCommand(PlayExecute);
            ResetFramesCommand = new DelegateCommand(ResetFramesExecute);
            FrameLeftCommand = new DelegateCommand(FrameLeftExecute);
            FrameRightCommand = new DelegateCommand(FrameRightExecute);
            ToggleVisiblityCommand = new RelayCommand(x => ToggleVisiblityExecute(x));
            RenderScale = new ScaleTransform();
            ZoomIndex = AppSettings.Instance.PreviewZoomIndex;

            _layerSprites = new List<DrawSpriteModel>();
            _recordedSprites = new HashSet<string>();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / 60.0);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            IsPlaying = true;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (!IsPlaying)
                return;

            foreach(var item in Sprites)
            {
                int prevIndex = item.Sprite.RegionIndex;
                if (IsPlaying)
                    item.Sprite.Update(_timer.Interval.TotalMilliseconds);
            }
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
            _recordedSprites.Clear();

            int zind = 0;
            foreach (var item in _selectedItems)
            {
                if (item is SpriteModel sprite && !_recordedSprites.Contains(sprite.Name))
                {
                    Sprites.Add(new DrawSpriteModel
                    {
                        Sprite = sprite,
                        ZIndex = zind++
                    });
                    _recordedSprites.Add(sprite.Name);
                }
            }

            foreach (var item in _layerSprites)
            {
                item.ZIndex = zind++;
                Sprites.Add(item);
            }
        }

        private void ToggleVisiblityExecute(object parameters)
        {
            if (Visibility == Visibility.Visible)
                Visibility = Visibility.Hidden;
            else
                Visibility = Visibility.Visible;
        }

        private void ClosingExecute(object parameters)
        {
            var args = (CancelEventArgs)parameters;

            args.Cancel = true;
            Visibility = Visibility.Hidden;
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

        private void ResetFramesExecute()
        {
            foreach(var spr in Sprites)
            {
                spr.Sprite.ResetFrames();
            }
        }

        private void FrameLeftExecute()
        {
            foreach(var spr in Sprites)
            {
                spr.Sprite.RegionIndex = spr.Sprite.RegionIndex - 1;
            }
        }

        private void FrameRightExecute()
        {
            foreach(var spr in Sprites)
            {
                spr.Sprite.RegionIndex = spr.Sprite.RegionIndex + 1;
            }
        }
    }
}
