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
using craftersmine.Packager.Lib.Core;
using craftersmine.Packager.Lib.Core.Exceptions;
using System.Threading;

namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    public partial class BuilderProgressForm : Form
    {
        public string PackageFilePath { get; set; }

        public BuilderProgressForm()
        {
            InitializeComponent();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "craftersmine Package (*.cmpkg)|*.cmpkg";
            saveFileDialog.Title = "Select file saving path...";
            switch (saveFileDialog.ShowDialog())
            {
                case DialogResult.Cancel:
                    MessageBox.Show("Package creation canceled by user!", "Operation canceled by user.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    break;
                case DialogResult.OK:
                    PackageFilePath = saveFileDialog.FileName;
                    if (PackageFilePath != string.Empty && PackageFilePath != "" && PackageFilePath != null)
                    {
                        Pack();
                    }
                    break;
            }
        }

        string tempDir = Path.Combine(Path.GetTempPath(), "~cgepackager");

        public void Pack()
        {
            Thread _workingThread = new Thread(new ThreadStart(() => {
                try
                {
                    int totalProgress = StaticData.ContentAssets.Count * 2 + 2;
                    int percentageCount = 0;
                    int perc = 0;
                    ChangeStatus("Preparing...");
                    ChangePercentage("0%");
                    Directory.CreateDirectory(tempDir);
                    PackageFile packageFile = new PackageFile(Path.GetFileNameWithoutExtension(PackageFilePath));
                    string packageDir = Path.GetDirectoryName(PackageFilePath);
                    foreach (var entry in StaticData.ContentAssets)
                    {
                        string ext = ".unk";
                        switch (entry.Value.ContentType)
                        {
                            case ContentType.Texture:
                                ext = ".tex";
                                break;
                            case ContentType.AnimationMetadata:
                                ext = ".amd";
                                break;
                            case ContentType.Animation:
                                ext = ".tex";
                                break;
                            case ContentType.Font:
                                ext = ".fnt";
                                break;
                            case ContentType.Strings:
                                ext = ".strings";
                                break;
                            case ContentType.WaveAudio:
                                ext = ".wad";
                                break;
                        }
                        string filename = "unknown";
                        filename = entry.Key.Replace(" [Animation]", "") + ext;
                        string tempFilePath = Path.Combine(tempDir, filename);
                        ChangeStatus("Copying " + entry.Value.AssetName + " to temporary folder...");
                        File.Copy(entry.Value.AssetPath, tempFilePath);
                        percentageCount++;
                        ChangeStatus("Adding " + entry.Value.AssetName + " to package...");
                        packageFile.AddFile(tempFilePath);
                        percentageCount++;
                        perc = (totalProgress / 100) * percentageCount;
                        ChangePercentage(perc.ToString() + "%");
                        UpdateProgress1(perc);
                    }
                    ChangeStatus("Packing package...");
                    Packager.Lib.Core.Packager packer = new Packager.Lib.Core.Packager(packageDir, packageFile);
                    packer.PackingDoneEvent += Packer_PackingDoneEvent;
                    packer.PackingEvent += Packer_PackingEvent;
                    packer.Pack();
                }
                catch (Exception)
                {
                    Directory.Delete(tempDir, true);
                    ChangeStatus("Packing package... Failed!");
                    ChangePercentage("100%");
                    MessageBox.Show("Error while packing! Packing canceled!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (InvokeRequired)
                        Invoke(new Action(() => { this.Close(); }));
                    else this.Close();
                }
            }));
        }

        private void Packer_PackingEvent(object sender, PackingEventArgs e)
        {
            UpdateProgress2(e.PackingPercentage);
        }

        private void Packer_PackingDoneEvent(object sender, PackingDoneEventArgs e)
        {
            if (e.IsSuccessful)
            {
                Directory.Delete(tempDir, true);
                ChangeStatus("Packing package... Done!");
                ChangePercentage("100%");
                switch (MessageBox.Show("Packing successful! Operation complete!", "Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information))
                {
                    case DialogResult.OK:
                        if (InvokeRequired)
                            Invoke(new Action(() => { this.Close(); }));
                        else this.Close();
                        break;
                }
            }
            else
            {
                Directory.Delete(tempDir, true);
                ChangeStatus("Packing package... Failed!");
                ChangePercentage("100%");
                switch (MessageBox.Show("Error while packing! Packing canceled!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error))
                {
                    case DialogResult.OK:
                        if (InvokeRequired)
                            Invoke(new Action(() => { this.Close(); }));
                        else this.Close();
                        break;
                }
            }
        }

        private void ChangeStatus(string status)
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    label1.Text = status;
                }));
            else label1.Text = status;
        }

        private void ChangePercentage(string perc)
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    label2.Text = perc;
                }));
            else label2.Text = perc;
        }

        private void UpdateProgress1(int perc)
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    progressBar1.Value = perc;
                }));
            else progressBar1.Value = perc;
        }

        private void UpdateProgress2(int perc)
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    progressBar2.Value = perc;
                }));
            else progressBar2.Value = perc;
        }
    }
}
