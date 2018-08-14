using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Content;
using craftersmine.GameEngine.System;

namespace craftersmine.GE.Utilities.GameObjectEditor
{
    public partial class MainForm : Form
    {
        public static EditorGameObject DummyObject = new EditorGameObject() { Width = 64, Height = 64 };

        public static string ObjectFilePath = string.Empty;

        public static EditorContentStorage ContentStorage;

        public static Dictionary<string, string> SaveData = new Dictionary<string, string>();

        public MainForm(string preOpen)
        {
            InitializeComponent();
            scene.SetBackgroundColor(Color.LightBlue);
            scene.OnCreate();
            redrawCaller.Start();
            scene.AddGameObject(DummyObject);
            DummyObject.ApplyTexture(new Texture(Properties.Resources.noTexture, TextureLayout.Tile));
            scene.UpdateCameraBounds();
            propertyGrid1.SelectedObject = DummyObject;
            DummyObject.X = scene.Width / 2 - DummyObject.Width / 2;
            DummyObject.Y = scene.Height / 2 - DummyObject.Height / 2;
            SaveData.Add("GAMEOBJDATA", "1");
            SaveData.Add("WIDTH", DummyObject.Width.ToString());
            SaveData.Add("HEIGHT", DummyObject.Height.ToString());
            SaveData.Add("COLLSET", "0,0,0,0");
            SaveData.Add("ID", "0");
            SaveData.Add("INTNAME", "");
            ObjectFilePath = preOpen;
            if (ObjectFilePath != string.Empty)
            {
                string[] readed = File.ReadAllLines(ObjectFilePath);
                ParseData(readed);
            }
        }

        private void redrawCaller_Tick(object sender, EventArgs e)
        {
            scene.CallDraw();
            DummyObject.OnUpdate();
            DummyObject.InternalUpdate();
            SaveData["ID"] = DummyObject.Id.ToString();
            SaveData["INTNAME"] = DummyObject.InternalName;
            SaveData["HEIGHT"] = DummyObject.Height.ToString();
            SaveData["WIDTH"] = DummyObject.Width.ToString();
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            DummyObject.X = scene.Width / 2 - DummyObject.Width / 2;
            DummyObject.Y = scene.Height / 2 - DummyObject.Height / 2;
        }

        private void fromFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "craftersmine GameEngine Animation Metadata (*.amd)|*.amd|All Files|*.*"
            };
            switch (dialog.ShowDialog())
            {
                case DialogResult.OK:
                    string texPath = Path.ChangeExtension(dialog.FileName, ".tex");
                    Texture animTexture = new Texture(Image.FromFile(texPath), TextureLayout.Default);
                    string[] animationMetadata = File.ReadAllLines(dialog.FileName);
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
                        }
                    }
                    DummyObject.ApplyAnimation(new Animation(animTexture, animFrmCount, animFrmDuration, frameWidth));
                    break;
                default: break;
            }
        }

        private void loadContentPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContentStorage == null)
            {
                LoadContentPackage();   
            }
            else
            {
                switch (MessageBox.Show("Content Package \"" + ContentStorage.PackageName + "\" is currently loaded. Do you want to unload it and select other package?", "Selecting other package", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        LoadContentPackage();
                        break;
                    default:break;
                }
            }
        }

        private void LoadContentPackage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select content package",
                Filter = "craftersmine GameEngine Content package (*.gep)|*.gep",
                Multiselect = false
            };
            switch (openFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    ContentStorage = new EditorContentStorage(openFileDialog.FileName);
                    fromPackageToolStripMenuItem.Enabled = true;
                    fromPackageToolStripMenuItem1.Enabled = true;
                    break;
                default: break;
            }
        }

        private void fromPackageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectFromPackageForm selectFrom = new SelectFromPackageForm(ContentType.Animation);
            switch (selectFrom.ShowDialog())
            {
                case DialogResult.OK:
                    DummyObject.ApplyAnimation(ContentStorage.LoadAnimation(selectFrom.SelectedObjectName));
                    break;
                default: break;
            }
        }

        private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "craftersmine GameEngine Texture (*.tex)|*.tex|Portable Network Graphics (*.png)|*.png|JPEG Images (*.jpg;*.jpeg)|*.jpg;*.jpeg|CompuServe GIF (*.gif)|*.gif|Windows Bitmap Format (*.bmp)|*.bmp|All Files (*.*)|*.*",
                Multiselect = false,
                Title = "Select texture file"
            };
            switch (openFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    DummyObject.ApplyTexture(new Texture(Image.FromFile(openFileDialog.FileName), TextureLayout.Default));
                    break;
                default: break;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"craftersmine GameEngine{Environment.NewLine}{Environment.NewLine}Game Object Editor Utility{Environment.NewLine}{Environment.NewLine}Copyright (c) craftersmine 2018{Environment.NewLine}{Environment.NewLine}Licensed Under \"MIT Licence\"", "About utility", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void fromPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFromPackageForm selectFrom = new SelectFromPackageForm(ContentType.Texture);
            switch (selectFrom.ShowDialog())
            {
                case DialogResult.OK:
                    DummyObject.ApplyTexture(ContentStorage.LoadTexture(selectFrom.SelectedObjectName, selectFrom.TextureLayout));
                    break;
                default: break;
            }
        }

        private void setBoundingBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBoundingBoxForm setBounding = new SetBoundingBoxForm();
            switch (setBounding.ShowDialog())
            {
                case DialogResult.OK:
                    DummyObject.SetCollider(setBounding.Xoff, setBounding.Yoff, setBounding.BBoxWidth, setBounding.BBoxHeight);
                    SaveData["COLLSET"] = setBounding.Xoff + "," + setBounding.Yoff + "," + setBounding.BBoxWidth + "," + setBounding.BBoxHeight;
                    break;
                default: break;
            }
        }

        private void setTextureInterpolationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetInterpolationModeForm setInterpolation = new SetInterpolationModeForm(scene.TextureInterpolationMode);
            switch (setInterpolation.ShowDialog())
            {
                case DialogResult.OK:
                    scene.TextureInterpolationMode = setInterpolation.InterpolationMode;
                    break;
                default: break;
            }
            scene.CallDraw();
        }

        private void newGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("Do you want to save game object before creating new?", "Creating new game object", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
            {
                case DialogResult.Yes:
                    SaveObject();
                    CreateNewObject();
                    break;
                case DialogResult.No:
                    CreateNewObject();
                    break;
                case DialogResult.Cancel: break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (MessageBox.Show("Do you want to save game object before exit?", "Exiting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
            {
                case DialogResult.Yes:
                    SaveObject();
                    break;
                case DialogResult.No:

                    break;
                case DialogResult.Cancel: e.Cancel = true; break;
            }
        }

        private void SaveObject()
        {
            if (ObjectFilePath == string.Empty)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "craftersmine GameEngine Game Object Editor Object File (*.gameobj)|*.gameobj",
                    Title = "Select file to save"
                };
                switch (saveFileDialog.ShowDialog())
                {
                    case DialogResult.OK:
                        ObjectFilePath = saveFileDialog.FileName;
                        break;
                    case DialogResult.Cancel: break;
                }
            }
            List<string> file = new List<string>();
            foreach (var savedataEntry in SaveData)
            {
                string ln = savedataEntry.Key + "=" + savedataEntry.Value;
                file.Add(ln);
            }
            File.WriteAllLines(ObjectFilePath, file);
        }

        private void CreateNewObject()
        {
            redrawCaller.Stop();
            scene.RemoveGameObject(DummyObject);
            SaveData.Clear();
            DummyObject = null;
            DummyObject = new EditorGameObject() { Width = 64, Height = 64 };
            scene.AddGameObject(DummyObject);
            DummyObject.ApplyTexture(new Texture(Properties.Resources.noTexture, TextureLayout.Tile));
            scene.UpdateCameraBounds();
            propertyGrid1.SelectedObject = DummyObject;
            DummyObject.X = scene.Width / 2 - DummyObject.Width / 2;
            DummyObject.Y = scene.Height / 2 - DummyObject.Height / 2;
            SaveData.Add("GAMEOBJDATA", "1");
            SaveData.Add("WIDTH", DummyObject.Width.ToString());
            SaveData.Add("HEIGHT", DummyObject.Height.ToString());
            SaveData.Add("COLLSET", "0,0,0,0");
            SaveData.Add("ID", "0");
            SaveData.Add("INTNAME", "");
            redrawCaller.Start();
        }

        private void saveGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveObject();
        }

        private void openGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("Do you want to save game object before opening existing?", "Opening existing object", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
            {
                case DialogResult.Yes:
                    SaveObject();
                    CreateNewObject();
                    OpenObject();
                    break;
                case DialogResult.No:
                    CreateNewObject();
                    OpenObject();
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }

        private void OpenObject()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "craftersmine GameEngine Game Object Editor Object File(*.gameobj)|*.gameobj",
                Multiselect = false,
                Title = "Select game object file"
            };
            switch (openFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    redrawCaller.Stop();
                    string[] loaded = File.ReadAllLines(openFileDialog.FileName);
                    if (loaded[0] == "GAMEOBJDATA=1")
                    {
                        ParseData(loaded);
                    }
                    else
                    {
                        MessageBox.Show("Unable to open game object data file! This file is not valid data file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    redrawCaller.Start();
                    break;
                case DialogResult.Cancel: break;
            }
        }

        private void ParseData(string[] data)
        {
            SaveData.Clear();
            foreach (var ln in data)
            {
                string[] splittedLine = ln.Split('=');
                SaveData.Add(splittedLine[0], splittedLine[1]);
            }
            DummyObject.Id = int.Parse(SaveData["ID"]);
            DummyObject.InternalName = SaveData["INTNAME"];
            DummyObject.Height = int.Parse(SaveData["HEIGHT"]);
            DummyObject.Width = int.Parse(SaveData["WIDTH"]);
            string[] collData = SaveData["COLLSET"].Split(',');
            int xOff = int.Parse(collData[0]);
            int yOff = int.Parse(collData[1]);
            int width = int.Parse(collData[2]);
            int height = int.Parse(collData[3]);
            DummyObject.SetCollider(xOff, yOff, width, height);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Exporter().ShowDialog();
        }

        private void gitHubWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/craftersmine/craftersmine-GameEngine/wiki");
        }
    }
}
