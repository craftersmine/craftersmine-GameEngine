using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Content;

namespace craftersmine.GE.Utilities.GameObjectEditor
{
    public partial class SelectFromPackageForm : Form
    {
        private ContentType type;

        public string SelectedObjectName { get; private set; }
        public TextureLayout TextureLayout { get; private set; }

        public SelectFromPackageForm(ContentType type)
        {
            InitializeComponent();
            label1.Text = label1.Text.Replace("{resource}", type.ToString().ToLower());
            listView1.Items.Clear();
            this.type = type;
            string[] names = new string[] { };
            switch (type)
            {
                case ContentType.Texture:
                    names = MainForm.ContentStorage.GetTexturesNames();
                    label2.Visible = true;
                    comboBox1.Visible = true;
                    break;
                case ContentType.Animation:
                    names = MainForm.ContentStorage.GetAnimationsNames();
                    break;
            }
            int icon = 0;
            if (type == ContentType.Texture)
                icon = 1;
            foreach (var name in names)
            {
                listView1.Items.Add(new ListViewItem(new string[] { null, name }, icon));
            }
            comboBox1.SelectedItem = "Default";
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            SelectedObjectName = e.Item.SubItems[1].Text;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Default": TextureLayout = TextureLayout.Default; break;
                case "Stretch": TextureLayout = TextureLayout.Stretch; break;
                case "Tile": TextureLayout = TextureLayout.Tile; break;
                case "Center": TextureLayout = TextureLayout.Center; break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Default": TextureLayout = TextureLayout.Default; break;
                case "Stretch": TextureLayout = TextureLayout.Stretch; break;
                case "Tile": TextureLayout = TextureLayout.Tile; break;
                case "Center": TextureLayout = TextureLayout.Center; break;
            }
            this.Close();
        }
    }
}
