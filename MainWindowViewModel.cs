using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor
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

        public bool UnsavedChanges { get; set; }

        public SpritePreviewWindow SpritePreviewWindow { get; } = new SpritePreviewWindow();

        public MainWindowViewModel()
        {
            Clear();
        }

        public void Clear()
        {
            FileName = "Untitled";
            SavePath = "";
            UnsavedChanges = false;
            SpritePreviewWindow.DataContext = null;
        }
    }
}
