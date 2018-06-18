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
    public partial class AddContentForm : Form
    {
        public AddContentForm()
        {
            InitializeComponent();
            StaticData.SelectFileAndNameForm = new SelectFileAndNameForm();
        }

        public ContentType ContentType { get; private set; }

        private void button2_Click(object sender, EventArgs e)
        {
            StaticData.SelectFileAndNameForm = new SelectFileAndNameForm();
            StaticData.SelectFileAndNameForm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaticData.IsContentAddCanceled = true;
            StaticData.CallEvent();
            this.Close();
        }

        private void tex_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.Texture;
        }

        private void anim_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.Animation;
        }

        private void animMd_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.AnimationMetadata;
        }

        private void font_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.Font;
        }

        private void aud_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.WaveAudio;
        }

        private void str_CheckedChanged(object sender, EventArgs e)
        {
            StaticData.WizardContentAsset.ContentType = ContentType.Strings;
        }
    }

    public enum ContentType
    {
        Animation = 2, AnimationMetadata = 3, Font = 4, WaveAudio = 1, Texture = 0, Strings = 5
    }
}
