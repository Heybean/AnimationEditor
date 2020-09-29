using DungeonSphere.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationManager.Graphics
{
    public class WpfTextureRegion : TextureAtlasData.Region
    {
        /// <summary>
        /// The texture atlas the sprite is from
        /// </summary>
        public BitmapImage TextureAtlas { get; private set; }

        /// <summary>
        /// The cropped sprite from the texture atlas
        /// </summary>
        public ImageBrush Sprite { get; private set; }

        public WpfTextureRegion(BitmapImage atlas, TextureAtlasData.Region region)
        {
            page = region.page;
            top = region.top;
            left = region.left;
            width = region.width;
            height = region.height;
            name = region.name;
            offsetX = region.offsetX;
            offsetY = region.offsetY;
            originalWidth = region.originalWidth;
            originalHeight = region.originalHeight;
            rotate = region.rotate;
            flip = region.flip;
            splits = region.splits;
            pads = region.pads;

            TextureAtlas = atlas;
            Sprite = new ImageBrush(TextureAtlas);
            Sprite.Viewport = new Rect(0, 0, 1, 1);
            //RenderOptions.SetBitmapScalingMode(Sprite, BitmapScalingMode.NearestNeighbor);
            //Sprite.ViewportUnits = BrushMappingMode.Absolute;
            //Sprite.Viewport = new Rect(left, top, width, height);
        }
    }
}
