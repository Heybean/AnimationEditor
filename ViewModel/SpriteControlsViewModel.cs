using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AnimationEditor.Model;
using PropertyTools.Wpf;

namespace AnimationEditor.ViewModel
{
    public class SpriteControlsViewModel : ViewModelBase
    {
        private int _selectedLayerIndex;
        private IList<object> _selectedAtlasItems;
        private HashSet<string> _setLayers;

        public ICommand AddLayerCommand { get; }
        public ICommand RemoveLayerCommand { get; }
        public ICommand MoveLayerUpCommand { get; }
        public ICommand MoveLayerDownCommand { get; }
        public ICommand ClearLayersCommand { get; }

        public ObservableCollection<SpriteModel> SpriteLayers { get; set; }
        public int SelectedLayerIndex
        {
            get => _selectedLayerIndex;
            set
            {
                _selectedLayerIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsDraggable => throw new NotImplementedException();

        public delegate void LayersUpdatedEventHandler(object sender, EventArgs args);
        public event LayersUpdatedEventHandler OnUpdateLayers;
        public event LayersUpdatedEventHandler OnUpdateLayersZIndex;

        public SpriteControlsViewModel()
        {
            AddLayerCommand = new RelayCommand(x => AddLayerExecute(null));
            RemoveLayerCommand = new RelayCommand(x => RemoveLayerExecute(x));
            MoveLayerUpCommand = new RelayCommand(x => MoveLayerUpExecute(x));
            MoveLayerDownCommand = new RelayCommand(x => MoveLayerDownExecute(x));

            ClearLayersCommand = new DelegateCommand(ClearLayersExecute);

            SpriteLayers = new ObservableCollection<SpriteModel>();
            _setLayers = new HashSet<string>();
        }

        public void TextureAtlasVM_SelectionChanged(object sender, EventArgs e)
        {
            _selectedAtlasItems = e.Parameters as IList<object>;
        }

        public void MoveLayerUpExecute(object parameters)
        {
            var selectedItems = (IList<object>)parameters;
            if (selectedItems.Count != 1)
                return;

            var spr = selectedItems[0] as SpriteModel;

            if (spr == null)
                return;

            var ind = SpriteLayers.IndexOf(spr);

            if (ind == 0)
                return;

            // do the swap. Doing this wierd insert thing because swapping by index causes dictionary key error
            var tmp = SpriteLayers[ind - 1];
            SpriteLayers.RemoveAt(ind - 1);
            SpriteLayers.Remove(spr);
            SpriteLayers.Insert(ind - 1, tmp);
            SpriteLayers.Insert(ind - 1, spr);
            SelectedLayerIndex = ind - 1;

            OnUpdateLayersZIndex?.Invoke(this, new EventArgs());
        }

        public void MoveLayerDownExecute(object parameters)
        {
            var selectedItems = (IList<object>)parameters;
            if (selectedItems.Count != 1)
                return;

            var spr = selectedItems[0] as SpriteModel;

            if (spr == null)
                return;

            var ind = SpriteLayers.IndexOf(spr);

            if (ind >= SpriteLayers.Count - 1)
                return;

            // do the swap. Doing this wierd insert thing because swapping by index causes dictionary key error
            var tmp = SpriteLayers[ind + 1];
            SpriteLayers.RemoveAt(ind + 1);
            SpriteLayers.Remove(spr);
            SpriteLayers.Insert(ind, tmp);
            SpriteLayers.Insert(ind + 1, spr);
            SelectedLayerIndex = ind + 1;

            OnUpdateLayersZIndex?.Invoke(this, new EventArgs());
        }

        private void ClearLayersExecute()
        {
            SpriteLayers.Clear();
            _setLayers.Clear();
            OnUpdateLayers?.Invoke(this, new EventArgs { Parameters = SpriteLayers });
        }

        private void AddLayerExecute(object parameters)
        {
            if (_selectedAtlasItems == null)
                return;

            bool updated = false;
            foreach(var item in _selectedAtlasItems)
            {
                if (item is SpriteModel sprite)
                {
                    if (!_setLayers.Contains(sprite.Name))
                    {
                        SpriteLayers.Add(sprite);
                        _setLayers.Add(sprite.Name);
                        updated = true;
                    }
                }
            }

            if (updated)
                OnUpdateLayers?.Invoke(this, new EventArgs { Parameters = SpriteLayers });
        }

        private void RemoveLayerExecute(object parameters)
        {
            var list = (IList<object>)parameters;

            bool removed = false;

            foreach(var item in list.ToList())
            {
                if (item is SpriteModel sprite)
                {
                    removed = true;
                    SpriteLayers.Remove(sprite);
                    _setLayers.Remove(sprite.Name);
                }
            }

            if (removed)
                OnUpdateLayers?.Invoke(this, new EventArgs { Parameters = SpriteLayers });
        }


    }
}
