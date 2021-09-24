using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class MainViewModel
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

        public TextureAtlasesViewModel TextureAtlasesViewModel { get; }
        public MainCanvasViewModel MainCanvasViewModel { get; }

        public MainViewModel()
        {
            TextureAtlasesViewModel = new TextureAtlasesViewModel();
            MainCanvasViewModel = new MainCanvasViewModel();
        }

        /*public void Clear()
        {
            FileName = "Untitled";
            SavePath = "";
            UnsavedChanges = false;
            SpritePreviewWindow.DataContext = null;
        }*/
    }
}
