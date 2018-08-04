using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GE.Utilities.GameObjectEditor
{
    public partial class Exporter : Form
    {
        public Exporter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Copy();
            textBox1.DeselectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nl = Environment.NewLine;
            string ctor = $"this.Id = $ID;{nl}this.InternalName = \"$INTNAME\";{nl}this.Width = $WIDTH;{nl}this.Height = $HEIGHT;{nl}{nl}// ------- Set Collider -------{nl}this.SetCollider($COLLSET);";
            foreach (var saveentry in MainForm.SaveData)
            {
                ctor = ctor.Replace("$" + saveentry.Key, saveentry.Value);
            }
            textBox1.Text = ctor;
        }
    }
}
