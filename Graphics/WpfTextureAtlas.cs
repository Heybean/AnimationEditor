using DungeonSphere.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationManager.Graphics
{
    public class WpfTextureAtlas : TextureAtlas
    {
        public List<BitmapImage> Textures { get; } = new List<BitmapImage>();
        public ObservableCollection<WpfSprite> Sprites { get; } = new ObservableCollection<WpfSprite>();

        public WpfTextureAtlas(string packFile)
        {
            var atlasData = new TextureAtlasData(packFile, false);

            Name = Path.GetFileNameWithoutExtension(packFile);

            // Load the bitmap images
            foreach (var page in atlasData.Pages)
            {
                var uri = new Uri(Path.GetFullPath(page.textureName + ".png"));
                Textures.Add(new BitmapImage(uri));
            }

            // Create the sprite animations for each set of animations
            var regionMaps = new Dictionary<string, List<WpfTextureRegion>>();
            foreach (var region in atlasData.Regions)
            {
                if (!regionMaps.ContainsKey(region.name))
                    regionMaps.Add(region.name, new List<WpfTextureRegion>());

                var wpfTextureRegion = new WpfTextureRegion(Textures[region.pageIndex], region);
                regionMaps[region.name].Add(wpfTextureRegion);
            }

            // Sort the indexes for proper animation order then create the sprite for it
            foreach(var entry in regionMaps)
            {
                entry.Value.Sort((x, y) => x.index.CompareTo(y.index));
                Sprites.Add(new WpfSprite(entry.Key, entry.Value));
            }
        }
    }
}
