using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.Content;
using Ionic.Zip;

namespace craftersmine.GE.Utilities.GameObjectEditor
{
    public class EditorContentStorage
    {
        private string PackagePath { get; set; }

        public string PackageName { get; internal set; }

        public EditorContentStorage(string packagePath)
        {
            PackageName = Path.GetFileNameWithoutExtension(packagePath);
            CreateContentStorage(packagePath);
        }

        public string[] GetTexturesNames()
        {
            try
            {
                List<string> names = new List<string>();
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    foreach (var entry in pak.Entries)
                    {
                        if (Path.GetExtension(entry.FileName) == ".tex")
                            names.Add(Path.GetFileNameWithoutExtension(entry.FileName));
                    }
                }
                return names.ToArray();
            }
            catch
            {
                throw new ContentLoadException("Unable to read package!");
            }
        }

        public string[] GetAnimationsNames()
        {
            try
            {
                List<string> names = new List<string>();
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    foreach (var entry in pak.Entries)
                    {
                        if (Path.GetExtension(entry.FileName) == ".amd")
                            names.Add(Path.GetFileNameWithoutExtension(entry.FileName));
                    }
                }
                return names.ToArray();
            }
            catch
            {
                throw new ContentLoadException("Unable to read package!");
            }
        }

        private void CreateContentStorage(string path)
        {
            PackagePath = path;
        }

        public Texture LoadTexture(string name, TextureLayout textureLayout)
        {
            try
            {
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".tex"].Extract(ms);
                    Texture tex = new Texture(Image.FromStream(ms), textureLayout);
                    return tex;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" texture from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }

        public Animation LoadAnimation(string name)
        {
            try
            {
                Texture texture = LoadTexture(name, TextureLayout.Stretch);
                using (ZipFile pak = ZipFile.Read(PackagePath))
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Position = 0;
                    pak[name + ".amd"].Extract(ms);
                    byte[] raw = ms.ToArray();
                    string[] animationMetadata = Encoding.Default.GetString(raw).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    int animFrmDuration = 0;
                    int animFrmCount = 0;
                    int frameWidth = 0;
                    foreach (var ln in animationMetadata)
                    {
                        string[] split = ln.Split('=');
                        switch (split[0].ToLower())
                        {
                            case "frameticktrigger":
                                if (!int.TryParse(split[1], out animFrmDuration))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                            case "framecount":
                                if (!int.TryParse(split[1], out animFrmCount))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                            case "framewidth":
                                if (!int.TryParse(split[1], out frameWidth))
                                    throw new ContentLoadException("Unable to load animation metadata of " + name + " from " + this.PackageName + "! Invalid metadata parameter value: \"" + split[0] + "=" + split[1] + "\" must be numerical Int32 value");
                                break;
                        }
                    }
                    Animation animation = new Animation(texture, animFrmCount, animFrmDuration, frameWidth);
                    return animation;
                }
            }
            catch (Exception ex)
            {
                throw new ContentLoadException("Unable to load \"" + name + "\" animation from " + this.PackageName + "! Inner exception message: " + ex.Message, ex);
            }
        }
    }
}
