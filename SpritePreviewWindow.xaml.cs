using AnimationManager.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnimationManager
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
            _spriteDisplay.Width = sprite.Regions[0].width;
            _spriteDisplay.Height = sprite.Regions[0].height;

            //_spriteDisplay.DataContext = _sprite;
            _previewRender.DataContext = _sprite;

            UpdatePreviewRender();
        }

        private void PreviewRender_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePreviewRender();
        }

        public void UpdatePreviewRender()
        {
            int x = (int)(_previewRender.ActualWidth - _spriteDisplay.Width) / 2;
            int y = (int)(_previewRender.ActualHeight - _spriteDisplay.Height) / 2;

            _previewRenderScale.CenterX = _spriteDisplay.Width / 2;
            _previewRenderScale.CenterY = _spriteDisplay.Height / 2;

            _spriteDisplay.RenderTransform = _previewRenderScale;

            Canvas.SetLeft(_spriteDisplay, x);
            Canvas.SetTop(_spriteDisplay, y);
        }
    }
}
