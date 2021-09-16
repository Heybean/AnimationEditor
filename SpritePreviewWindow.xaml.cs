using AnimationEditor.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimationEditor
{
    /// <summary>
    /// Interaction logic for SpritePreviewWindow.xaml
    /// </summary>
    public partial class SpritePreviewWindow : Window
    {
        private WpfSprite _sprite;
        private Canvas _previewRender;
        private Rectangle _spriteDisplay;
        private ScaleTransform _previewRenderScale;

        public SpritePreviewWindow()
        {
            InitializeComponent();

            _previewRender = (Canvas)FindName("cv_PreviewRender");
            _spriteDisplay = (Rectangle)FindName("rect_Sprite");

            RenderOptions.SetBitmapScalingMode(_spriteDisplay, BitmapScalingMode.NearestNeighbor);

            _previewRenderScale = new ScaleTransform(2, 2);
        }

        public void SetSprite(WpfSprite sprite)
        {
            _sprite = sprite;
            _previewRender.DataContext = _sprite;

            if (sprite != null)
            {
                _spriteDisplay.Width = sprite.Regions[0].width;
                _spriteDisplay.Height = sprite.Regions[0].height;

                UpdatePreviewRender();
            }
        }

        private void PreviewRender_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePreviewRender();
        }

        private void PreviewRender_Closing(object sender, CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        public void UpdatePreviewRender()
        {
            int x = (int)(_previewRender.ActualWidth) / 2;
            int y = (int)(_previewRender.ActualHeight) / 2;

            if (_sprite != null)
            {
                x -= _sprite.OriginX;
                y -= _sprite.OriginY;
            }

            _previewRenderScale.CenterX = _spriteDisplay.Width / 2;
            _previewRenderScale.CenterY = _spriteDisplay.Height / 2;

            _spriteDisplay.RenderTransform = _previewRenderScale;

            Canvas.SetLeft(_spriteDisplay, x);
            Canvas.SetTop(_spriteDisplay, y);
        }
    }
}
