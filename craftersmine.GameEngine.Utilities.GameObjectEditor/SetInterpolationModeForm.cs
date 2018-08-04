using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GE.Utilities.GameObjectEditor
{
    public partial class SetInterpolationModeForm : Form
    {
        public InterpolationMode InterpolationMode { get; private set; }

        public SetInterpolationModeForm(InterpolationMode mode)
        {
            InitializeComponent();
            switch (mode)
            {
                case InterpolationMode.Default: comboBox1.SelectedItem = "Default"; break;
                case InterpolationMode.High: comboBox1.SelectedItem = "High"; break;
                case InterpolationMode.Low: comboBox1.SelectedItem = "Low"; break;
                case InterpolationMode.HighQualityBicubic: comboBox1.SelectedItem = "High Quality Bicubic"; break;
                case InterpolationMode.HighQualityBilinear: comboBox1.SelectedItem = "High Quality Bilinear"; break;
                case InterpolationMode.NearestNeighbor: comboBox1.SelectedItem = "Nearest Neighbor"; break;
                case InterpolationMode.Bilinear: comboBox1.SelectedItem = "Bilinear"; break;
                case InterpolationMode.Bicubic: comboBox1.SelectedItem = "Bicubic"; break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString().ToLower())
            {
                case "default": InterpolationMode = InterpolationMode.Default; break;
                case "high": InterpolationMode = InterpolationMode.High; break;
                case "low": InterpolationMode = InterpolationMode.Low; break;
                case "high quality bilinear": InterpolationMode = InterpolationMode.HighQualityBilinear; break;
                case "high quality bicubic": InterpolationMode = InterpolationMode.HighQualityBicubic; break;
                case "nearest neighbor": InterpolationMode = InterpolationMode.NearestNeighbor; break;
                case "bilinear": InterpolationMode = InterpolationMode.Bilinear; break;
                case "bicubic": InterpolationMode = InterpolationMode.Bicubic; break;
            }
            this.Close();
        }
    }
}
