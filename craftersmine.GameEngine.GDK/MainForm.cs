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
using craftersmine.GDK.Core;

namespace craftersmine.GDK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "craftersmine GDK Project (*.gdkproj)|*.gdkproj",
                Title = "Select project file to be opened",
                Multiselect = false
            };
            switch (dialog.ShowDialog())
            {
                case DialogResult.OK:
                    try
                    {
                        StaticData.LoadedProject = new Project(dialog.FileName);
                        StaticData.LoadedProject.LoadProject();
                        status.Text = "Project loaded: " + StaticData.LoadedProject.ProjectName;
                        bgTaskDesc.Text = StaticData.LoadedProject.Files[0].FileContents[0];
                    }
                    catch (Exception ex)
                    {
                        status.Text = "Unable to load project \"" + dialog.FileName + "\": " + ex.Message;
                    }
                    break;
                default: break;
            }
        }
    }
}
