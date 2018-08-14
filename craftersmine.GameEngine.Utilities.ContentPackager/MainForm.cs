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

namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    public partial class MainForm : Form
    {
        public MainForm(string preLoad)
        {
            this.InitializeComponent();
            StaticData.ContentAssetAddedEvent += new StaticData.ContentAssetAddedEventDelegate(this.StaticData_ContentAssetAdded);
            FilePath = preLoad;
            if (FilePath != string.Empty)
            {
                LoadProject();
                this.UpdateList();
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            StaticData.AddContentForm = new AddContentForm();
            base.Enabled = false;
            StaticData.AddContentForm.Show();
        }

        private static void CallSaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "craftersmine GameEngine Content Package Builder project (*.cgepproj)|*.cgepproj"
            };
            switch (dialog.ShowDialog())
            {
                case DialogResult.OK:
                    FilePath = dialog.FileName;
                    SaveProjData();
                    break;
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            StaticData.ContentAssets.Clear();
            this.UpdateList();
        }

        private void create_Click(object sender, EventArgs e)
        {
            BuilderProgressForm builderProgressForm = new BuilderProgressForm();
            //builderProgressForm.ShowDialog();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                this.removeBtn.Enabled = true;
            }
            else
            {
                this.removeBtn.Enabled = false;
            }
        }

        public static void LoadProject()
        {
            string[] strArray = File.ReadAllLines(FilePath);
            foreach (string str in strArray)
            {
                ContentAsset asset2;
                char[] separator = new char[] { '=' };
                string[] strArray3 = str.Split(separator);
                char[] chArray2 = new char[] { '|' };
                string[] strArray4 = strArray3[1].Split(chArray2);
                string key = strArray3[0];
                string str3 = strArray4[0];
                ContentType strings = ContentType.Strings;
                string str4 = strArray4[1].ToLower();
                if (!(str4 == "texture"))
                {
                    if (str4 == "animation")
                    {
                        goto Label_00B9;
                    }
                    if (str4 == "animationmetadata")
                    {
                        goto Label_00BE;
                    }
                    if (str4 == "font")
                    {
                        goto Label_00C3;
                    }
                    if (str4 == "waveaudio")
                    {
                        goto Label_00C8;
                    }
                    if (str4 == "strings")
                    {
                        goto Label_00CD;
                    }
                }
                else
                {
                    strings = ContentType.Texture;
                }
                goto Label_00D2;
                Label_00B9:
                strings = ContentType.Animation;
                goto Label_00D2;
                Label_00BE:
                strings = ContentType.AnimationMetadata;
                goto Label_00D2;
                Label_00C3:
                strings = ContentType.Font;
                goto Label_00D2;
                Label_00C8:
                strings = ContentType.WaveAudio;
                goto Label_00D2;
                Label_00CD:
                strings = ContentType.Strings;
                Label_00D2:
                asset2 = new ContentAsset();
                asset2.ContentType = strings;
                asset2.AssetName = key;
                asset2.AssetPath = str3;
                ContentAsset asset = asset2;
                StaticData.ContentAssets.Add(key, asset);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticData.ContentAssets.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to save this project?", "Creating new project...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        if (FilePath != string.Empty)
                        {
                            SaveProject(false);
                        }
                        else
                        {
                            SaveProject(true);
                        }
                        StaticData.ContentAssets.Clear();
                        this.UpdateList();
                        return;
                }
                if (result == DialogResult.No)
                {
                    StaticData.ContentAssets.Clear();
                    this.UpdateList();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "craftersmine GameEngine Content Package Builder project (*.cgepproj)|*.cgepproj"
            };
            if (StaticData.ContentAssets.Count <= 0)
            {
                StaticData.ContentAssets.Clear();
                switch (dialog.ShowDialog())
                {
                    case DialogResult.OK:
                        FilePath = dialog.FileName;
                        LoadProject();
                        break;
                }
                this.UpdateList();
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want to save this project?", "Creating new project...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        if (FilePath != string.Empty)
                        {
                            SaveProject(false);
                        }
                        else
                        {
                            SaveProject(true);
                        }
                        StaticData.ContentAssets.Clear();
                        switch (dialog.ShowDialog())
                        {
                            case DialogResult.OK:
                                FilePath = dialog.FileName;
                                LoadProject();
                                break;
                        }
                        this.UpdateList();
                        return;
                }
                if (result == DialogResult.No)
                {
                    StaticData.ContentAssets.Clear();
                    switch (dialog.ShowDialog())
                    {
                        case DialogResult.OK:
                            FilePath = dialog.FileName;
                            LoadProject();
                            break;
                    }
                    this.UpdateList();
                }
            }
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if ((this.listView1.SelectedItems.Count > 0) && StaticData.ContentAssets.ContainsKey(this.listView1.SelectedItems[0].SubItems[1].Text))
            {
                StaticData.ContentAssets.Remove(this.listView1.SelectedItems[0].SubItems[1].Text);
            }
            this.UpdateList();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject(true);
        }

        public static void SaveProjData()
        {
            List<string> contents = new List<string>();
            foreach (KeyValuePair<string, ContentAsset> pair in StaticData.ContentAssets)
            {
                string[] textArray1 = new string[] { pair.Value.AssetName, "=", pair.Value.AssetPath, "|", pair.Value.ContentType.ToString() };
                string item = string.Concat(textArray1);
                contents.Add(item);
            }
            File.WriteAllLines(FilePath, contents);
        }

        public static void SaveProject(bool saveAs)
        {
            if (FilePath != string.Empty)
            {
                if (!File.Exists(FilePath))
                {
                    CallSaveAs();
                }
                else if (saveAs)
                {
                    CallSaveAs();
                }
                else
                {
                    SaveProjData();
                }
            }
            else
            {
                CallSaveAs();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject(false);
        }

        private void StaticData_ContentAssetAdded(object sender, ContentAssetAddedEventArgs e)
        {
            if (!StaticData.IsContentAddCanceled)
            {
                this.UpdateList();
                create.Enabled = true;
            }
            else
            {
                StaticData.WizardContentAsset = new ContentAsset();
            }
            base.Enabled = true;
        }

        private void UpdateList()
        {
            this.listView1.Enabled = false;
            this.addBtn.Enabled = false;
            this.clearBtn.Enabled = false;
            this.create.Enabled = false;
            this.fileToolStripMenuItem.Enabled = false;
            this.listView1.Cursor = Cursors.WaitCursor;
            this.listView1.Items.Clear();
            foreach (KeyValuePair<string, ContentAsset> pair in StaticData.ContentAssets)
            {
                int imageIndex = 0;
                imageIndex = (int)pair.Value.ContentType;
                string[] items = new string[] { "", pair.Value.AssetName, pair.Value.AssetPath };
                this.listView1.Items.Add(new ListViewItem(items, imageIndex));
            }
            this.listView1.Cursor = Cursors.Default;
            this.listView1.Enabled = true;
            this.listView1.Enabled = true;
            this.addBtn.Enabled = true;
            this.clearBtn.Enabled = true;
            this.create.Enabled = true;
            this.fileToolStripMenuItem.Enabled = true;
            base.Enabled = true;
        }

        public static string FilePath { get; set; }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticData.ContentAssets.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to save this project?", "Exiting...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        if (FilePath != string.Empty)
                        {
                            SaveProject(false);
                        }
                        else
                        {
                            SaveProject(true);
                        }
                        StaticData.ContentAssets.Clear();
                        return;
                }
                if (result == DialogResult.No)
                {
                    StaticData.ContentAssets.Clear();
                }
            }
            this.Close();
        }
    }
}
