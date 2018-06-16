using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.Packager.Lib.Core;
using craftersmine.Packager.Lib.Core.Exceptions;
using craftersmine.GameEngine.Utils;

namespace craftersmine.GameEngine.Content
{
    /// <summary>
    /// Represents game content storage
    /// </summary>
    public sealed class ContentStorage
    {
        private OnDemandPackage pak { get; set; }
        private string packageName { get; set; }

        /// <summary>
        /// Creates new <see cref="ContentStorage"/> instance with <paramref name="packageName"/>
        /// </summary>
        /// <param name="packageName">Package name without extention from "content" root game directory</param>
        public ContentStorage(string packageName)
        {
            this.packageName = packageName;
            CreateContentStorage();
        }
        
        private void CreateContentStorage()
        {
            pak = new OnDemandPackage(Path.Combine(Environment.CurrentDirectory, "content", packageName + ".cmpkg"));
            ContentStorageCreated?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Loads <see cref="Texture"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Texture"/></param>
        /// <returns><see cref="Texture"/></returns>
        public Texture LoadTexture(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Texture });
            try
            {
                byte[] imageRaw = pak.ReadBytes(name + ".tex");
                Image image = ImageFromBytesConverter.ByteArrayToImage(imageRaw);
                Texture tex = new Texture(image);
                return tex;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" texture! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="Animation"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Animation"/></param>
        /// <returns><see cref="Animation"/></returns>
        public Animation LoadAnimation(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Animation });
            try
            {
                Texture texture = LoadTexture(name);
                string[] animationMetadata = pak.ReadLines(name + ".amd");
                int animFrmDuration = 0;
                int animFrmCount = 0;
                int frameWidth = 0;
                bool isBackground = true;
                foreach (var ln in animationMetadata)
                {
                    string[] split = ln.Split('=');
                    switch (split[0].ToLower())
                    {
                        case "frameticktrigger":
                            if (!int.TryParse(split[1], out animFrmDuration))
                                throw new ContentLoadException("Unable to load animation metadata! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                            break;
                        case "framecount":
                            if (!int.TryParse(split[1], out animFrmCount))
                                throw new ContentLoadException("Unable to load animation metadata! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                            break;
                        case "framewidth":
                            if (!int.TryParse(split[1], out frameWidth))
                                throw new ContentLoadException("Unable to load animation metadata! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                            break;
                        case "isbackground":
                            if (!bool.TryParse(split[1], out isBackground))
                                throw new ContentLoadException("Unable to load animation metadata! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Boolean value");
                            break;
                    }
                }
                Animation animation = new Animation(texture, animFrmCount, animFrmDuration, frameWidth, isBackground);
                return animation;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" animation! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="Font"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Font"/></param>
        /// <param name="fontSize">Font size in pt</param>
        /// <returns><see cref="Font"/></returns>
        public Font LoadFont(string name, float fontSize)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Font });
            try
            {
                byte[] fontDataRaw = pak.ReadBytes(name + ".fnt");
                FontFamily fml = FontFromBytesConverter.FontFamilyFromBytes(fontDataRaw);
                Font font = new Font(fml, fontSize);
                return font;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" font! Inner exception message: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads <see cref="Audio"/> from package
        /// </summary>
        /// <param name="name">Name of <see cref="Audio"/></param>
        /// <returns><see cref="Audio"/></returns>
        public Audio LoadAudio(string name)
        {
            ContentLoading?.Invoke(this, new ContentLoadingEventArgs() { ContentFileName = name, ContentType = ContentType.Audio });
            try
            {
                byte[] audioDataRaw = pak.ReadBytes(name + ".wad");
                Audio audio = new Audio(WaveFileReaderFromBytesConverter.ByteArrayToWaveFileReader(audioDataRaw));
                return audio;
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" audio! Inner exception message: " + ex.Message, ex);
            }
        }

        public delegate void ContentStorageCreatedEventDelegate(object sender, EventArgs e);
        /// <summary>
        /// Calls at <see cref="ContentStorage"/> was created
        /// </summary>
        public event ContentStorageCreatedEventDelegate ContentStorageCreated;

        public delegate void ContentLoadingEventDelegate(object sender, ContentLoadingEventArgs e);
        /// <summary>
        /// Calls at loading any content from this <see cref="ContentStorage"/>
        /// </summary>
        public event ContentLoadingEventDelegate ContentLoading;
    }

    /// <summary>
    /// <see cref="ContentStorage.ContentLoading"/> event arguments
    /// </summary>
    public sealed class ContentLoadingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets loading content filename without extention
        /// </summary>
        public string ContentFileName { get; internal set; }
        //public bool IsExists { get; internal set; }
        /// <summary>
        /// Gets loading content file type
        /// </summary>
        public ContentType ContentType { get; internal set; }
    }

    /// <summary>
    /// Types of loadable content
    /// </summary>
    public enum ContentType
    {
        Texture, Animation, Font, Audio
    }
}
