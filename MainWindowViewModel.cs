using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationManager
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// File name without extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Full file path
        /// </summary>
        public string SavePath { get; set; }

        public SpritePreviewWindow SpritePreviewWindow { get; } = new SpritePreviewWindow();
    }
}
