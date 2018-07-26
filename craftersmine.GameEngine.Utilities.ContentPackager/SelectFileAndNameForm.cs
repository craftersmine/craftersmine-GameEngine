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
    public partial class SelectFileAndNameForm : Form
    {
        public SelectFileAndNameForm()
        {
            InitializeComponent();
            label1.Text = label1.Text.Replace("{ctype}", StaticData.WizardContentAsset.ContentType.ToString());
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.AssetPath = textBox1.Text;
            StaticData.WizardContentAsset.AssetName = textBox2.Text;
            if (StaticData.WizardContentAsset.ContentType == ContentType.AnimationMetadata)
            {
                StaticData.WizardContentAsset.AssetName += " [Animation]";
            }
            StaticData.ContentAssets.Add(StaticData.WizardContentAsset.AssetName, StaticData.WizardContentAsset);
            StaticData.IsContentAddCanceled = false;
            StaticData.CallEvent();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StaticData.IsContentAddCanceled = true;
            StaticData.CallEvent();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog browserDialog = new OpenFileDialog();
            switch (StaticData.WizardContentAsset.ContentType)
            {
                case ContentType.Texture:
                    browserDialog.Filter = "JPEG Images (*.jpg; *.jpeg)|*.jpg;*.jpeg|Portable Network Graphics Images (*.png)|*.png|GIF Images (*.gif)|*.gif|All Files|*.*";
                    browserDialog.Title = "Select texture image...";
                    break;
                case ContentType.AnimationMetadata:
                    browserDialog.Filter = "craftersmine GameEngine AnimationMeta File (*.amd)|*.amd";
                    browserDialog.Title = "Select texture animation metadata file...";
                    break;
                case ContentType.Font:
                    browserDialog.Filter = "TrueType Font|*.ttf";
                    browserDialog.Title = "Select TrueType Font...";
                    break;
                case ContentType.WaveAudio:
                    browserDialog.Filter = "Wave Audio Sample (*.wav)|*.wav";
                    browserDialog.Title = "Select Wave Audio Sample...";
                    break;
                case ContentType.Strings:
                    browserDialog.Filter = "craftersmine GameEngine Strings Key-Value File (*.strings)|*.strings";
                    browserDialog.Title = "Select strings Key-Value file...";
                    break;
            }
            browserDialog.Multiselect = false;
            switch (browserDialog.ShowDialog())
            {
                case DialogResult.OK:
                    textBox1.Text = browserDialog.FileName;
                    textBox2.Text = Path.GetFileNameWithoutExtension(textBox1.Text);
                    if (textBox2.Text.Length > 0 && textBox1.Text.Length > 0)
                        button3.Enabled = true;
                    else
                        button3.Enabled = false;
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0 && textBox1.Text.Length > 0)
                button3.Enabled = true;
            else
                button3.Enabled = false;
        }
    }
}
