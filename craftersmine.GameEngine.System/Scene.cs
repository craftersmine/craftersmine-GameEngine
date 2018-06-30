using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Content;
using craftersmine.GameEngine.Objects;
using RazorGDI;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Game scene. Represents game level, menu, or other.
    /// </summary>
    public class Scene : Panel
    {
        private Dictionary<string, AudioChannel> _audioChannels = new Dictionary<string, AudioChannel>();
        internal List<GameObject> GameObjects = new List<GameObject>();
        internal RazorPainterControl BaseCanvas { get; set; }

        /// <summary>
        /// Gets scene background texture
        /// </summary>
        public Texture BackgroundTexture { get; internal set; }
        
        /// <summary>
        /// Identifier of scene in game
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Scene"/>
        /// </summary>
        public Scene()
        {
            BaseCanvas = new RazorPainterControl();
            BaseCanvas.BackColor = Color.Transparent;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BaseCanvas.BackgroundImage = new Bitmap(this.Width, this.Height);
            this.Controls.Add(BaseCanvas);
            BaseCanvas.MouseClick += BaseCanvas_MouseClick;
            BaseCanvas.MouseUp += BaseCanvas_MouseUp;
            BaseCanvas.MouseMove += BaseCanvas_MouseMove;
        }

        private void BaseCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle clickPoint = new Rectangle(e.X, e.Y, 1, 1);
            foreach (var gObj in GameObjects)
            {
                if (clickPoint.IntersectsWith(gObj.BoundingBox))
                    gObj.OnMouseMove(e.X, e.Y, e.Button);
            }
        }

        private void BaseCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            Rectangle clickPoint = new Rectangle(e.X, e.Y, 1, 1);
            foreach (var gObj in GameObjects)
            {
                if (clickPoint.IntersectsWith(gObj.BoundingBox))
                    gObj.OnMouseUp(e.Button);
            }
        }

        private void BaseCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            Rectangle clickPoint = new Rectangle(e.X, e.Y, 1, 1);
            foreach (var gObj in GameObjects)
            {
                if (clickPoint.IntersectsWith(gObj.BoundingBox))
                    gObj.OnMouseClick(e.X, e.Y, e.Button);
            }
        }

        /// <summary>
        /// Adds <paramref name="gameObject"/> at this scene
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        /// <summary>
        /// Removes <paramref name="gameObject"/> from this scene
        /// </summary>
        /// <param name="gameObject"></param>
        public void RemoveGameObject(GameObject gameObject)
        {
            gameObject.OnDestroy();
            GameObjects.Remove(gameObject);
        }
        /// <summary>
        /// Sets background color of scene
        /// </summary>
        /// <param name="color">Background color</param>
        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }
        /// <summary>
        /// Sets background color of scene in RGB format
        /// </summary>
        /// <param name="red">Red color component</param>
        /// <param name="green">Green color component</param>
        /// <param name="blue">Blue color component</param>
        public void SetBackgroundColor(int red, int green, int blue)
        {
            this.BackColor = Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Sets scene background texture from <see cref="Texture"/> with layout <paramref name="textureLayout"/>
        /// </summary>
        /// <param name="texture">Texture data to set</param>
        /// <param name="textureLayout">Texture layout on scene</param>
        public void SetBackgroundTexture(Texture texture, ImageLayout textureLayout)
        {
            this.BackgroundTexture = texture;
            this.BackgroundImage = texture.TextureImage;
            this.BackgroundImageLayout = textureLayout;
        }
        /// <summary>
        /// Sets scene background texture from <see cref="Texture"/> with <see cref="ImageLayout.Stretch"/> layout
        /// </summary>
        /// <param name="texture">Texture data to set</param>
        public void SetBackgroundTexture(Texture texture)
        {
            SetBackgroundTexture(texture, ImageLayout.Stretch);
        }

        /// <summary>
        /// Adds <see cref="AudioChannel"/> to scene
        /// </summary>
        /// <param name="audioChannel"><see cref="AudioChannel"/> to add</param>
        public void AddAudioChannel(AudioChannel audioChannel)
        {
            _audioChannels.Add(audioChannel.ChannelName, audioChannel);
        }

        /// <summary>
        /// Plays <see cref="AudioChannel"/>
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        public void PlayAudioChannel(string name)
        {
            _audioChannels[name].Play();
        }

        /// <summary>
        /// Stops playing <see cref="AudioChannel"/>
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        public void StopAudioChannel(string name)
        {
            _audioChannels[name].Stop();
        }

        /// <summary>
        /// Pauses playing <see cref="AudioChannel"/>
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        public void PauseAudioChannel(string name)
        {
            _audioChannels[name].Pause();
        }

        /// <summary>
        /// Resumes paused <see cref="AudioChannel"/>
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        public void ResumeAudioChannel(string name)
        {
            _audioChannels[name].Resume();
        }

        /// <summary>
        /// Returns true if <see cref="AudioChannel"/> is playing, else false
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        /// <returns><see cref="Boolean"/></returns>
        public bool IsAudioChannelPlaying(string name)
        {
            return _audioChannels[name].IsPlaying;
        }

        /// <summary>
        /// Returns true if <see cref="AudioChannel"/> is paused, else false
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        /// <returns><see cref="Boolean"/></returns>
        public bool IsAudioChannelPaused(string name)
        {
            return _audioChannels[name].IsPaused;
        }

        /// <summary>
        /// Sets <see cref="AudioChannel"/> output device
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        /// <param name="deviceNumber">Output device number</param>
        public void SetAudioChannelOutputDevice(string name, int deviceNumber)
        {
            _audioChannels[name].SetOutputDevice(deviceNumber);
        }

        /// <summary>
        /// Sets <see cref="AudioChannel"/> looping
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        /// <param name="repeating">If true, sets <see cref="AudioChannel"/> loop, else false</param>
        public void SetAudioChannelRepeat(string name, bool repeating)
        {
            _audioChannels[name].IsRepeating = repeating;
        }

        /// <summary>
        /// Sets <see cref="AudioChannel"/> volume
        /// </summary>
        /// <param name="name">Name of <see cref="AudioChannel"/></param>
        /// <param name="volume">Volume of <see cref="AudioChannel"/> in 0.0f to 1.0f range</param>
        public void SetAudioChannelVolume(string name, float volume)
        {
            if (volume > 1.0f)
                _audioChannels[name].ChannelVolume = 1.0f;
            else if (volume < 0.0f)
                _audioChannels[name].ChannelVolume = 0.0f;
            else _audioChannels[name].ChannelVolume = volume;
        }

        internal void StopAllAudioChannels()
        {
            foreach (var chl in _audioChannels)
            {
                chl.Value.Stop();
            }
        }

        internal Dictionary<string, AudioChannel> GetAudioChannels()
        {
            return _audioChannels;
        }

        /// <summary>
        /// Calls at scene creation
        /// </summary>
        public virtual void OnCreate()
        {
            BaseCanvas.Size = new Size(this.Width, this.Height);
        }
        /// <summary>
        /// Calls at game update
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        internal void Draw()
        {
            lock (BaseCanvas.RazorLock)
            {
                BaseCanvas.RazorGFX.Clear(this.BackColor);
                if (this.BackgroundTexture != null)
                {
                    //BaseCanvas.RazorGFX.DrawImage(BackgroundTexture.TextureImage, 0, 0);
                    DrawTexture(BackgroundTexture, 0, 0, this.Width, this.Height, null);
                }
                foreach (var gObj in GameObjects)
                {
                    if (gObj.CurrentTexture != null)
                    {
                        DrawTexture(gObj.CurrentTexture, gObj.X, gObj.Y, gObj.Width, gObj.Height, gObj);
                    }
                }
                BaseCanvas.RazorPaint();
            }
        }

        private void DrawTexture(Texture texture, int xPos, int yPos, int width, int height, GameObject gameObject)
        {
            Rectangle textureBounding = new Rectangle(xPos, yPos, width, height);
            switch (texture.TextureLayout)
            {
                case TextureLayout.Stretch:
                    BaseCanvas.RazorGFX.DrawImage(texture.TextureImage, textureBounding);
                    break;
                case TextureLayout.Tile:
                    int xCount = width / texture.TextureImage.Width + 1;
                    int yCount = height / texture.TextureImage.Height + 1;
                    Bitmap tiledTex = new Bitmap(width, height);
                    Graphics painter = Graphics.FromImage(tiledTex);
                    painter.InterpolationMode = TextureInterpolationMode;
                    for (int x = 0; x < xCount; x++)
                        for (int y = 0; y < yCount; y++)
                        {
                            painter.DrawImage(texture.TextureImage, texture.TextureImage.Width * x, texture.TextureImage.Height * y);
                        }
                    BaseCanvas.RazorGFX.DrawImage(tiledTex, xPos, yPos);
                    break;
                case TextureLayout.Center:
                    int xCenter = (width / 2) - (texture.TextureImage.Width / 2);
                    int yCenter = (height / 2) - (texture.TextureImage.Height / 2);
                    textureBounding.X = xCenter;
                    textureBounding.Y = yCenter;
                    textureBounding.Width = texture.TextureImage.Width;
                    textureBounding.Height = texture.TextureImage.Height;
                    BaseCanvas.RazorGFX.DrawImage(texture.TextureImage, textureBounding);
                    break;
            }
        }
    }
}
