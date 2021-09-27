using AnimationEditor.Graphics;
using AnimationEditor.IO;
using AnimationEditor.Model;
using AnimationEditor.View;
using Heybean.Graphics;
using Microsoft.Win32;
using PropertyTools;
using PropertyTools.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AnimationEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// File name without extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Full file path
        /// </summary>
        private string SavePath { get; set; }

        public bool UnsavedChanges { get; private set; }

        //public SpritePreviewWindow SpritePreviewWindow { get; } = new SpritePreviewWindow();

        public TextureAtlasesViewModel TextureAtlasesVM { get; }
        public MainCanvasViewModel MainCanvasVM { get; }
        public SpritePropertiesViewModel SpritePropertiesVM { get; }

        public ICommand ClosingCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand SpritePreviewCommand { get; }

        public MainViewModel()
        {
            TextureAtlasesVM = new TextureAtlasesViewModel();
            MainCanvasVM = new MainCanvasViewModel();
            SpritePropertiesVM = new SpritePropertiesViewModel();

            TextureAtlasesVM.SelectionChanged += MainCanvasVM.TextureAtlasSelectionChanged;
            TextureAtlasesVM.SelectionChanged += SpritePropertiesVM.TextureAtlasSelectionChanged;

            TextureAtlasesVM.OnFileModified += FileModifiedEvent;
            SpritePropertiesVM.OnFileModified += FileModifiedEvent;

            SpritePropertiesVM.OnUpdateOriginMarker += MainCanvasVM.OnOriginUpdated;

            ClosingCommand = new RelayCommand(x => ClosingExecute(x));
            ExitCommand = new RelayCommand(x => ExitExecute(x));
            NewCommand = new DelegateCommand(NewExecute);
            OpenCommand = new DelegateCommand(OpenExecute);
            SaveCommand = new DelegateCommand(SaveExecute);
            SaveAsCommand = new DelegateCommand(SaveAsExecute);
            SpritePreviewCommand = new RelayCommand(x => SpritePreviewExecute(x));

            Reset();
        }

        private void FileModifiedEvent(object sender)
        {
            UnsavedChanges = true;
        }

        private void ClosingExecute(object parameters)
        {
            var e = (CancelEventArgs)parameters;

            if (!PromptUnsavedChanges())
                e.Cancel = true;
        }

        private void ExitExecute(object parameters)
        {
            var window = (Window)parameters;
            window.Close();
        }

        private void Reset()
        {
            FileName = "Untitled.anim";
            SavePath = "";
            TextureAtlasesVM.Reset();
            SpritePropertiesVM.Reset();
            UnsavedChanges = false;
        }

        private void NewExecute()
        {
            if (PromptUnsavedChanges())
            {
                Reset();
            }
        }

        private void OpenExecute()
        {
            if (!PromptUnsavedChanges())
                return;

            var openFileDialog = new OpenFileDialog()
            {
                Title = "Open File",
                Filter = "animation files (*.anim)|*.anim"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Reset();
                SavePath = openFileDialog.FileName;
                FileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                var data = FileReader.Read(openFileDialog.FileName);
                LoadData(openFileDialog.FileName, data);
            }
        }

        private void LoadData(string filename, AnimationsFileData data)
        {
            var rootFolder = Path.GetDirectoryName(filename);
            TextureAtlasesVM.Root.Name = FileName + ".anim";

            var atlasDict = new Dictionary<string, SpriteModel>();
            foreach (var atlasData in data.Root.Atlases)
            {
                var atlas = TextureAtlasesVM.AddTextureAtlas(rootFolder + "\\" + atlasData.File);
                foreach (SpriteModel sprite in atlas.SubNodes)
                {
                    atlasDict.Add(sprite.Name, sprite);
                }

                RecreateStructure(atlasData, TextureAtlasesVM.Root, atlasDict);
            }

            UnsavedChanges = false;
        }

        private void RecreateStructure(AnimationsFileData.Folder parentFolder, Node parentNode, Dictionary<string, SpriteModel> atlasDict)
        {
            Node node = new Node(parentNode);
            foreach(var folder in parentFolder.Folders)
            {
                Node folderNode = new Node(node);
                RecreateStructure(folder, folderNode, atlasDict);
                node.SubNodes.Add(folderNode);
            }

            foreach(var spriteData in parentFolder.Sprites)
            {
                SpriteModel sprite;
                atlasDict.TryGetValue(spriteData.Name, out sprite);
                if (sprite == null)
                    continue;

                // Update the sprite with data from file
                sprite.FPS = spriteData.FPS;
                sprite.OriginX = spriteData.OriginX;
                sprite.OriginY = spriteData.OriginY;
                sprite.HAlign = (SpriteHorizontalAlignment)Enum.Parse(typeof(SpriteHorizontalAlignment), spriteData.HorizontalAlignment, true);
                sprite.VAlign = (SpriteVerticalAlignment)Enum.Parse(typeof(SpriteVerticalAlignment), spriteData.VerticalAlignment, true);

                node.SubNodes.Add(sprite);
            }
        }

        private void SaveExecute()
        {
            PerformSave();
        }

        private void SaveAsExecute()
        {
            PerformSaveFile(FileName);
            TextureAtlasesVM.Root.Name = FileName + ".anim";
        }

        private void SpritePreviewExecute(object parameters)
        {
            var window = (Window)parameters;

            var popup = CheckIfPopupOpen();

            if (popup == null)
            {
                popup = new SpritePreviewView();
                popup.Owner = window;
                popup.Show();
            }
            else
            {
                popup.Close();
            }
        }

        private Window CheckIfPopupOpen()
        {
            foreach(var window in Application.Current.Windows)
            {
                if (window is SpritePreviewView)
                    return (Window)window;
            }

            return null;
        }

        /// <summary>
        /// Prompts user to save file if unsaved changes exists.
        /// </summary>
        /// <returns>True if OK to close app. False if dialog was cancelled.</returns>
        private bool PromptUnsavedChanges()
        {
            // No unsaved changes detected
            if (!UnsavedChanges)
                return true;

            // Prompt for saving
            var result = MessageBox.Show($"Do you want to save changes to {FileName}.anim? Unsaved changes will be lost!", "Save File?", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return PerformSave();
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }

            return true;
        }

        private bool PerformSave()
        {
            if (SavePath.Length <= 0)
            {
                return PerformSaveFile("Save File");
            }
            else
            {
                FileWriter.Write(SavePath, TextureAtlasesVM.Root);
                UnsavedChanges = false;
                return true;
            }
        }

        private bool PerformSaveFile(string title)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = "animation files (*.anim)|*.anim",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SavePath = saveFileDialog.FileName;
                FileName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                FileWriter.Write(SavePath, TextureAtlasesVM.Root);
                UnsavedChanges = false;

                return true;
            }

            return false;
        }
    }
}
