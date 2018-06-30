using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Content
{
    /// <summary>
    /// Represents texture
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Gets <see cref="Texture"/> image
        /// </summary>
        public Image TextureImage { get; private set; }

        public TextureLayout TextureLayout { get; private set; }

        /// <summary>
        /// Creates new <see cref="Texture"/> instance with <paramref name="textureImage"/>
        /// </summary>
        /// <param name="textureImage"><see cref="Image"/> for <see cref="Texture"/></param>
        public Texture(Image textureImage, TextureLayout textureLayout)
        {
            TextureImage = textureImage;
            TextureLayout = textureLayout;
        }

        /// <summary>
        /// Updates <see cref="Texture"/> image
        /// </summary>
        /// <param name="textureImage"><see cref="Image"/> for <see cref="Texture"/></param>
        public void UpdateTexture(Image textureImage, TextureLayout textureLayout)
        {
            TextureImage = textureImage;
            TextureLayout = textureLayout;
        }
    }

    public enum TextureLayout
    {
        Stretch, 
        Tile,
        Center
    }
}
