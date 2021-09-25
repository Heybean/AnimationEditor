using PropertyTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimationEditor.ViewModel
{
    public class MainViewModel : Observable
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

        //public SpritePreviewWindow SpritePreviewWindow { get; } = new SpritePreviewWindow();

        public TextureAtlasesViewModel TextureAtlasesVM { get; }
        public MainCanvasViewModel MainCanvasVM { get; }
        public SpritePropertiesViewModel SpritePropertiesVM { get; }

        public MainViewModel()
        {
            TextureAtlasesVM = new TextureAtlasesViewModel();
            MainCanvasVM = new MainCanvasViewModel();
            SpritePropertiesVM = new SpritePropertiesViewModel();

            TextureAtlasesVM.SelectionChanged += MainCanvasVM.TextureAtlasSelectionChanged;
            TextureAtlasesVM.SelectionChanged += SpritePropertiesVM.TextureAtlasSelectionChanged;
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
