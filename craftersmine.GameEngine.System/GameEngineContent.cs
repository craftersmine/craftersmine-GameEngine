using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.Content;

namespace craftersmine.GameEngine.System
{
    public sealed class GameEngineContent
    {
        public static ContentStorage SystemContent { get; internal set; }

        public static Texture LightCookieCircle { get { return SystemContent.LoadTexture("lightcookie_circle", TextureLayout.Stretch); } }
        public static Texture LightCookieSquare { get { return SystemContent.LoadTexture("lightcookie_square", TextureLayout.Stretch); } }
    }
}
