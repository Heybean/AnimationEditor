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

        public TextureAtlasesViewModel TextureAtlasesVM { get; }
        public MainCanvasViewModel MainCanvasVM { get; }
        public SpritePropertiesViewModel SpritePropertiesVM { get; }
        public SpritePreviewViewModel SpritePreviewVM { get; }
        public SpriteControlsViewModel SpriteControlsVM { get; }

        public ICommand ClosingCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand LoadedCommand { get; }

        public MainViewModel()
        {
            TextureAtlasesVM = new TextureAtlasesViewModel();
            MainCanvasVM = new MainCanvasViewModel();
            SpritePropertiesVM = new SpritePropertiesViewModel();
            SpriteControlsVM = new SpriteControlsViewModel();
            SpritePreviewVM = new SpritePreviewViewModel();

            TextureAtlasesVM.SelectionChanged += MainCanvasVM.TextureAtlasesVM_SelectionChanged;
            TextureAtlasesVM.SelectionChanged += SpritePropertiesVM.TextureAtlasesVM_SelectionChanged;
            TextureAtlasesVM.SelectionChanged += SpritePreviewVM.TextureAtlasesVM_SelectionChanged;
            TextureAtlasesVM.SelectionChanged += SpriteControlsVM.TextureAtlasVM_SelectionChanged;

            SpriteControlsVM.OnUpdateLayers += SpritePreviewVM.SpriteControls_LayersUpdated;

            TextureAtlasesVM.OnFileModified += FileModifiedEvent;
            SpritePropertiesVM.OnFileModified += FileModifiedEvent;

            SpritePropertiesVM.OnUpdateOriginMarker += MainCanvasVM.OnOriginUpdated;

            ClosingCommand = new RelayCommand(x => ClosingExecute(x));
            ExitCommand = new RelayCommand(x => ExitExecute(x));
            NewCommand = new DelegateCommand(NewExecute);
            OpenCommand = new DelegateCommand(OpenExecute);
            SaveCommand = new DelegateCommand(SaveExecute);
            SaveAsCommand = new DelegateCommand(SaveAsExecute);
            LoadedCommand = new DelegateCommand(LoadedExecute);

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

            AppSettings.Write();
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

        private void LoadedExecute()
        {
            var popup = new SpritePreviewView();
            popup.Owner = Application.Current.MainWindow;
            popup.DataContext = SpritePreviewVM;
            popup.Show();

            SpritePreviewVM.Visibility = AppSettings.Instance.PreviewVisibility;
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

        public void LoadData(string filename, AnimationsFileData data)
        {
            TextureAtlasesViewModel.LoadData(TextureAtlasesVM, filename, data);

            UnsavedChanges = false;
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

        /*public void SetAppProperties(AppSettings prop)
        {
            MainCanvasVM.ZoomIndex = prop.ZoomIndex;
        }

        public void GetAppProperties(ref AppSettings prop)
        {
            prop.ZoomIndex = MainCanvasVM.ZoomIndex;
        }*/
    }
}
