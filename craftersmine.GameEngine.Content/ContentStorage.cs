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
    public sealed class ContentStorage
    {
        private OnDemandPackage pak { get; set; }
        private string packageName { get; set; }

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
        public event ContentStorageCreatedEventDelegate ContentStorageCreated;

        public delegate void ContentLoadingEventDelegate(object sender, ContentLoadingEventArgs e);
        public event ContentLoadingEventDelegate ContentLoading;
    }

    public sealed class ContentLoadingEventArgs : EventArgs
    {
        public string ContentFileName { get; internal set; }
        //public bool IsExists { get; internal set; }
        public ContentType ContentType { get; internal set; }
    }

    public enum ContentType
    {
        Texture, Animation, Font, Audio
    }
}
