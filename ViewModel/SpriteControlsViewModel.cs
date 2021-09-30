using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using AnimationEditor.Model;
using PropertyTools.Wpf;

namespace AnimationEditor.ViewModel
{
    public class SpriteControlsViewModel : ViewModelBase
    {
        private IList<object> _selectedItems;
        private HashSet<string> _setLayers;

        public ICommand AddLayerCommand { get; }
        public ICommand ClearLayersCommand { get; }

        public ObservableCollection<SpriteModel> SpriteLayers { get; set; }

        public bool IsDraggable => throw new NotImplementedException();

        public delegate void LayersUpdatedEventHandler(object sender, EventArgs args);
        public event LayersUpdatedEventHandler OnUpdateLayers;

        public SpriteControlsViewModel()
        {
            AddLayerCommand = new RelayCommand(x => AddLayerExecute(null));
            ClearLayersCommand = new DelegateCommand(ClearLayersExecute);

            SpriteLayers = new ObservableCollection<SpriteModel>();
            _setLayers = new HashSet<string>();
        }

        public void TextureAtlasVM_SelectionChanged(object sender, EventArgs e)
        {
            _selectedItems = e.Parameters as IList<object>;
        }

        private void ClearLayersExecute()
        {
            SpriteLayers.Clear();
            _setLayers.Clear();
            OnUpdateLayers?.Invoke(this, new EventArgs { Parameters = SpriteLayers });
        }

        private void AddLayerExecute(object parameters)
        {
            if (_selectedItems == null)
                return;

            bool updated = false;
            foreach(SpriteModel sprite in _selectedItems)
            {
                if (!_setLayers.Contains(sprite.Name))
                {
                    SpriteLayers.Add(sprite);
                    _setLayers.Add(sprite.Name);
                    updated = true;
                }
            }

            if (updated)
                OnUpdateLayers?.Invoke(this, new EventArgs { Parameters = SpriteLayers });
        }
    }
}
