using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Content;
using craftersmine.GameEngine.Objects;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Game scene. Represents game level, menu, or other.
    /// </summary>
    public class Scene : Panel
    {
        private Dictionary<string, AudioChannel> _audioChannels = new Dictionary<string, AudioChannel>();

        /// <summary>
        /// Identifier of scene in game
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="Scene"/>
        /// </summary>
        /// <param name="id">Scene identifier in game</param>
        public Scene(int id)
        {
            Id = id;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Adds <paramref name="gameObject"/> at this scene
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObject.BringToFront();
            gameObject.OnCreate();
            this.Controls.Add(gameObject);
        }
        /// <summary>
        /// Removes <paramref name="gameObject"/> from this scene
        /// </summary>
        /// <param name="gameObject"></param>
        public void RemoveGameObject(GameObject gameObject)
        {
            gameObject.OnDestroy();
            this.Controls.Remove(gameObject);
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

        public void AddAudioChannel(AudioChannel audioChannel)
        {
            _audioChannels.Add(audioChannel.ChannelName, audioChannel);
        }

        public void PlayAudioChannel(string name)
        {
            _audioChannels[name].Play();
        }

        public void StopAudioChannel(string name)
        {
            _audioChannels[name].Stop();
        }

        public void PauseAudioChannel(string name)
        {
            _audioChannels[name].Pause();
        }

        public void ResumeAudioChannel(string name)
        {
            _audioChannels[name].Resume();
        }

        public bool IsAudioChannelPlaying(string name)
        {
            return _audioChannels[name].IsPlaying;
        }

        public bool IsAudioChannelPaused(string name)
        {
            return _audioChannels[name].IsPaused;
        }

        public void SetAudioChannelOutputDevice(string name, int deviceNumber)
        {
            _audioChannels[name].SetOutputDevice(deviceNumber);
        }

        public void SetAudioChannelRepeat(string name, bool repeating)
        {
            _audioChannels[name].IsRepeating = repeating;
        }

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

        }
        /// <summary>
        /// Calls at game update
        /// </summary>
        public virtual void OnUpdate()
        {

        }
    }
}
