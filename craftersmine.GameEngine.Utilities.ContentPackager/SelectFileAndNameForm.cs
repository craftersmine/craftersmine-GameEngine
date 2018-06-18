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
            browserDialog.Multiselect = false;
            switch (browserDialog.ShowDialog())
            {
                case DialogResult.OK:
                    textBox1.Text = browserDialog.FileName;
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
