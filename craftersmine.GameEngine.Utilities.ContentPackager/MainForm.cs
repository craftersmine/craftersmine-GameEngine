using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StaticData.ContentAssetAddedEvent += StaticData_ContentAssetAdded;
        }

        string selectedAsset = "";

        private void StaticData_ContentAssetAdded(object sender, ContentAssetAddedEventArgs e)
        {
            if (!StaticData.IsContentAddCanceled)
                UpdateList();
            else StaticData.WizardContentAsset = new ContentAsset();
            this.Enabled = true;
        }

        private void UpdateList()
        {
            listView1.Enabled = false;
            addBtn.Enabled = false;
            clearBtn.Enabled = false;
            create.Enabled = false;
            fileToolStripMenuItem.Enabled = false;
            listView1.Cursor = Cursors.WaitCursor;
            listView1.Items.Clear();
            foreach (var asset in StaticData.ContentAssets)
            {
                int imgId = 0;
                imgId = (int)asset.Value.ContentType;
                listView1.Items.Add(new ListViewItem(new string[] { "", asset.Value.AssetName, asset.Value.AssetPath }, imgId));
            }
            listView1.Cursor = Cursors.Default;
            listView1.Enabled = true;
            listView1.Enabled = true;
            addBtn.Enabled = true;
            clearBtn.Enabled = true;
            create.Enabled = true;
            fileToolStripMenuItem.Enabled = true;
            this.Enabled = true;
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (StaticData.ContentAssets.ContainsKey(listView1.SelectedItems[0].SubItems[1].Text))
                    StaticData.ContentAssets.Remove(listView1.SelectedItems[0].SubItems[1].Text);
            }
            UpdateList();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                removeBtn.Enabled = true;
            else removeBtn.Enabled = false;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            StaticData.AddContentForm = new AddContentForm();
            this.Enabled = false;
            StaticData.AddContentForm.Show();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            StaticData.ContentAssets.Clear();
            UpdateList();
        }

        private void create_Click(object sender, EventArgs e)
        {

        }
    }
}
