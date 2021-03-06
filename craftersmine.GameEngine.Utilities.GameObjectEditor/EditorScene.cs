﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using craftersmine.GameEngine.Content;
using craftersmine.GameEngine.Objects;
using RazorGDI;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Game scene. Represents game level, menu, or other.
    /// </summary>
    public class EditorScene : Panel
    {
        private Dictionary<string, AudioChannel> _audioChannels = new Dictionary<string, AudioChannel>();
        internal List<EditorGameObject> GameObjects { get; } = new List<EditorGameObject>();
        internal RazorPainterControl BaseCanvas { get; set; }
        private Pen texBoundingsRects = new Pen(Color.Gold);
        private Pen collBoundingsRects = new Pen(Color.Red);
        private SolidBrush dbgText = new SolidBrush(Color.MediumSpringGreen);

        private SolidBrush tintBrush;
        private Rectangle tinter;
        internal Rectangle CameraBounds;

        /// <summary>
        /// Gets scene background texture
        /// </summary>
        public Texture BackgroundTexture { get; internal set; }
        
        /// <summary>
        /// Identifier of scene in game
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets current texture interpolation mode
        /// </summary>
        public InterpolationMode TextureInterpolationMode { get; internal set; }

        /// <summary>
        /// Creates a new instance of <see cref="EditorScene"/>
        /// </summary>
        public EditorScene()
        {
            this.BaseCanvas = new RazorPainterControl();
            this.BaseCanvas.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            this.Controls.Add(BaseCanvas);
            this.BaseCanvas.MouseClick += BaseCanvas_MouseClick;
            this.BaseCanvas.MouseUp += BaseCanvas_MouseUp;
            this.BaseCanvas.MouseMove += BaseCanvas_MouseMove;
        }

        public void UpdateCameraBounds()
        {
            this.CameraBounds = new Rectangle(this.Location, this.Size);
        }

        private void BaseCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle clickPoint = new Rectangle(e.X, e.Y, 1, 1);
            foreach (var gObj in GameObjects)
            {
                if (clickPoint.IntersectsWith(gObj.BoundingBox))
                {
                    gObj.OnMouseMove(e.X, e.Y, e.Button);
                    //gObj.IsMouseOnObject = true;
                }
                else
                {
                    if (gObj.IsMouseOnObject)
                        gObj.OnMouseLeave();
                }
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
        public void AddGameObject(EditorGameObject gameObject)
        {
            GameObjects.Add(gameObject);
            gameObject.OnCreate();
        }

        /// <summary>
        /// Removes <paramref name="gameObject"/> from this scene
        /// </summary>
        /// <param name="gameObject"></param>
        public void RemoveGameObject(EditorGameObject gameObject)
        {
            GameObjects.Remove(gameObject);
            gameObject.OnDestroy();
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

        /// <summary>
        /// Removes tint from scene
        /// </summary>
        public void RemoveTint()
        {
            tinter.Size = new Size(0, 0);
        }

        /// <summary>
        /// Adds tint of scene with specified color and transparency value
        /// </summary>
        /// <param name="color">Tint color</param>
        /// <param name="transparency">Tint transparency value</param>
        public void TintScene(Color color, float transparency)
        {
            if (transparency > 1.0f || transparency < 0.0f)
                throw new ArgumentException("Transparency parameter must be in 0.0f to 1.0f range.", "transparency");
            int aN = (int)(transparency * 255);
            Color tClr = Color.FromArgb(aN, color);
            tintBrush = new SolidBrush(tClr);
            tinter = new Rectangle(0, 0, this.Width, this.Height);
        }

        /// <summary>
        /// Adds tint of scene with specified color RGB components and transparency value
        /// </summary>
        /// <param name="r">Tint Red color component</param>
        /// <param name="g">Tint Green color component</param>
        /// <param name="b">Tint Blue color component</param>
        /// <param name="transparency">Tint transparency value</param>
        public void TintScene(float r, float g, float b, float transparency)
        {
            if (r > 1.0f || r < 0.0f)
                throw new ArgumentException("Color component RED must be in 0.0f to 1.0f range.", "r");
            if (g > 1.0f || g < 0.0f)
                throw new ArgumentException("Color component GREEN must be in 0.0f to 1.0f range.", "g");
            if (b > 1.0f || b < 0.0f)
                throw new ArgumentException("Color component BLUE must be in 0.0f to 1.0f range.", "b");
            int rN = (int)(r * 255);
            int gN = (int)(g * 255);
            int bN = (int)(b * 255);
            Color clr = Color.FromArgb(rN, gN, bN);
            TintScene(clr, transparency);
        }

        /// <summary>
        /// Calls draw method of scene
        /// </summary>
        public void CallDraw()
        {
            Draw();
        }

        internal void Draw()
        {
            lock (BaseCanvas.RazorLock)
            {
                BaseCanvas.RazorGFX.InterpolationMode = TextureInterpolationMode;
                BaseCanvas.RazorGFX.Clear(this.BackColor);
                if (this.BackgroundTexture != null)
                {
                    DrawTexture(BackgroundTexture, 0, 0, this.Width, this.Height);
                }
                foreach (var gObj in GameObjects)
                {
                    if (gObj.CurrentTexture != null)
                    {
                        DrawGameObjectTexture(gObj);
                    }
                }
                if (tinter.Width != 0 && tinter.Height != 0)
                {
                    BaseCanvas.RazorGFX.FillRectangle(tintBrush, tinter);
                }
                BaseCanvas.RazorPaint();
            }
        }

        private void DrawGameObjectTexture(EditorGameObject gameObject)
        {
            if (gameObject != null)
            {
                if (gameObject.TextureBoundings.IntersectsWith(CameraBounds))
                {
                    switch (gameObject.CurrentTexture.TextureLayout)
                    {
                        case TextureLayout.Default:
                        case TextureLayout.Stretch:
                            BaseCanvas.RazorGFX.DrawImage(gameObject.CurrentTexture.TextureImage, gameObject.TextureBoundings);
                            break;
                        case TextureLayout.Center:
                            int xCenter = (gameObject.Width / 2) - (gameObject.CurrentTexture.TextureImage.Width / 2) + gameObject.X;
                            int yCenter = (gameObject.Height / 2) - (gameObject.CurrentTexture.TextureImage.Height / 2) + gameObject.Y;
                            BaseCanvas.RazorGFX.DrawImage(gameObject.CurrentTexture.TextureImage, xCenter, yCenter);
                            break;
                        case TextureLayout.Tile:
                            if (!gameObject.IsTiledTextureCached)
                            {
                                Texture tiledTex = PrepareTiledTexture(gameObject.CurrentTexture, gameObject.TextureBoundings);
                                gameObject.IsTiledTextureCached = true;
                                gameObject.TiledTextureCache = tiledTex;
                                BaseCanvas.RazorGFX.DrawImage(gameObject.TiledTextureCache.TextureImage, gameObject.TextureBoundings);
                            }
                            else BaseCanvas.RazorGFX.DrawImage(gameObject.TiledTextureCache.TextureImage, gameObject.TextureBoundings);
                            break;
                    }
                }
                BaseCanvas.RazorGFX.DrawRectangle(texBoundingsRects, gameObject.TextureBoundings);
                BaseCanvas.RazorGFX.DrawRectangle(collBoundingsRects, gameObject.BoundingBox);
            }
            else
            {
                //GameApplication.Log(Utils.LogEntryType.Warning, "GameEngine unable to draw game object!");
            }
        }

        private Texture PrepareTiledTexture(Texture texture, Rectangle textureBounds)
        {
            int xCount = textureBounds.Width / texture.TextureImage.Width + 1;
            int yCount = textureBounds.Height / texture.TextureImage.Height + 1;
            Bitmap tiledTexture = new Bitmap(textureBounds.Width, textureBounds.Height);
            Graphics painter = Graphics.FromImage(tiledTexture);
            painter.InterpolationMode = this.TextureInterpolationMode;
            //for (int xTile = 0; xTile < xCount; xTile++)
            //{
            //    int xTilePos = xTile * texture.TextureImage.Width;
            //    for (int yTile = 0; yTile < yCount; yTile++)
            //    {
            //        int yTilePos = yTile * texture.TextureImage.Height;
            //        painter.DrawImage(texture.TextureImage, xTilePos, yTilePos);
            //    }
            //}
            using (TextureBrush tiledBrush = new TextureBrush(texture.TextureImage))
                painter.FillRectangle(tiledBrush, new Rectangle(new Point(0, 0), textureBounds.Size));
            return new Texture(tiledTexture, TextureLayout.Tile);
        }

        private void DrawTexture(Texture texture, int xPos, int yPos, int width, int height)
        {
            Rectangle textureBounding = new Rectangle(xPos, yPos, width, height);
            switch (texture.TextureLayout)
            {
                case TextureLayout.Stretch:
                    BaseCanvas.RazorGFX.DrawImage(texture.TextureImage, textureBounding);
                    break;
                case TextureLayout.Tile:
                    Texture tiledTex = PrepareTiledTexture(texture, textureBounding);
                    BaseCanvas.RazorGFX.DrawImage(tiledTex.TextureImage, textureBounding);
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
