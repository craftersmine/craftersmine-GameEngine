using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Content
{
    public class Texture
    {
        public Image TextureImage { get; private set; }

        public Texture(Image textureImage)
        {
            TextureImage = textureImage;
        }

        public void UpdateTexture(Image textureImage)
        {
            TextureImage = textureImage;
        }
    }
}
