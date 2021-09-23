using Heybean.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnimationEditor.Graphics
{
    public class TextureRegion : TextureAtlasData.Region
    {
        /// <summary>
        /// The texture atlas the sprite is from
        /// </summary>
        public BitmapImage TextureAtlas { get; private set; }

        /// <summary>
        /// The cropped sprite from the texture atlas
        /// </summary>
        public ImageBrush ImageBrush { get; private set; }

        public TextureRegion(BitmapImage atlas, TextureAtlasData.Region region)
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
            ImageBrush = new ImageBrush(TextureAtlas);
            ImageBrush.ViewboxUnits = BrushMappingMode.Absolute;
            ImageBrush.Viewbox = new Rect(left, top, width, height);
        }
    }
}
