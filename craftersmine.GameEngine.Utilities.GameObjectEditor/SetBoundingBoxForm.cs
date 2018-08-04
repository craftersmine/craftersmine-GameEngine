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
    public partial class SetBoundingBoxForm : Form
    {
        public int Xoff { get; private set; }
        public int Yoff { get; private set; }
        public int BBoxWidth { get; private set; }
        public int BBoxHeight { get; private set; }

        public SetBoundingBoxForm()
        {
            InitializeComponent();
            xoff.Maximum = decimal.MaxValue;
            xoff.Minimum = decimal.MinValue;
            yoff.Maximum = decimal.MaxValue;
            yoff.Minimum = decimal.MinValue;
            bboxheight.Maximum = decimal.MaxValue;
            bboxheight.Minimum = decimal.MinValue;
            bboxwidth.Maximum = decimal.MaxValue;
            bboxwidth.Minimum = decimal.MinValue;
            xoff.Value = MainForm.DummyObject.ColliderOffsetX;
            yoff.Value = MainForm.DummyObject.ColliderOffsetY;
            bboxwidth.Value = MainForm.DummyObject.ColliderWidth;
            bboxheight.Value = MainForm.DummyObject.ColliderHeight;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Xoff = (int)xoff.Value;
            Yoff = (int)yoff.Value;
            BBoxHeight = (int)bboxheight.Value;
            BBoxWidth = (int)bboxwidth.Value;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
