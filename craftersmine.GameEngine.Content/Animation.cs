using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Content
{
    public class Animation
    {
        private List<Image> frames = new List<Image>();
        public Texture AnimationTexture { get; internal set; }
        public int AnimationFramesCount { get; internal set; }
        public int FrameTickTrigger { get; internal set; }
        public int CurrentFrame { get; set; }
        public int CountedTicks { get; set; }
        public int FrameWidth { get; internal set; }
        public bool IsBackgroundAnimation { get; internal set; }

        public Animation(Texture texture, int frames, int frameTickTrigger, int frameWidth, bool isBackground)
        {
            AnimationTexture = texture;
            AnimationFramesCount = frames;
            FrameTickTrigger = frameTickTrigger;
            FrameWidth = frameWidth;
            IsBackgroundAnimation = isBackground;
            PrepareAnimation();
        }

        public void PrepareAnimation()
        {
            for (int i = 0; i < AnimationFramesCount; i++)
            {
                frames.Add(new Bitmap(FrameWidth, AnimationTexture.TextureImage.Height));
                var image = Graphics.FromImage(frames[i]);
                image.DrawImage(AnimationTexture.TextureImage, new Rectangle(0, 0, FrameWidth, AnimationTexture.TextureImage.Height), new Rectangle(i * FrameWidth, 0, FrameWidth, AnimationTexture.TextureImage.Height), GraphicsUnit.Pixel);
                image.Dispose();
            }
        }

        public Image GetFrame(int frameId)
        {
            return frames[frameId];
        }
    }
}
