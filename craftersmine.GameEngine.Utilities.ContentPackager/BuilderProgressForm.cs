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
using System.Threading;
using Ionic.Zip;

namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    public partial class BuilderProgressForm : Form
    {
        public string PackageFilePath { get; set; }

        public BuilderProgressForm()
        {
            InitializeComponent();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "craftersmine GameEngine Package (*.gep)|*.gep",
                Title = "Select file saving path..."
            };
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
            Thread _workingThread = new Thread(new ThreadStart(Packer));
            _workingThread.Start();
        }

        private void Packer()
        {
            try
            {
                int percentageCount = 0;
                ChangeStatus("Preparing...");
                //ChangePercentage("0%");
                Directory.CreateDirectory(tempDir);
                ZipFile package = new ZipFile(PackageFilePath);
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
                    package.AddFile(tempFilePath, "");
                }
                ChangeStatus("Packing package...");
                package.SaveProgress += Package_SaveProgress;
                package.ZipError += Package_ZipError;
                package.Save();
            }
            catch (Exception ex)
            {
                Directory.Delete(tempDir, true);
                ChangeStatus("Packing package... Failed!");
                //ChangePercentage("100%");
                MessageBox.Show("Error while packing! Packing canceled! Message: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (InvokeRequired)
                    Invoke(new Action(() => { this.Close(); }));
                else this.Close();
            }
        }

        private void Package_ZipError(object sender, ZipErrorEventArgs e)
        {
            Thread.Sleep(10);
            Directory.Delete(tempDir, true);
            ChangeStatus("Packing package... Failed!");
            //ChangePercentage("100%");
            switch (MessageBox.Show("Error while packing! Packing canceled! Message: " + e.Exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                case DialogResult.OK:
                    if (InvokeRequired)
                        Invoke(new Action(() => { this.Close(); }));
                    else this.Close();
                    break;
            }
        }

        private void Package_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EventType != ZipProgressEventType.Saving_Started)
            {
                if (e.BytesTransferred > 0)
                {
                    int perc = (int)((e.TotalBytesToTransfer / 100) * e.BytesTransferred);
                    //UpdateProgress1(perc);
                    //ChangePercentage(perc.ToString() + "%");
                }
            }
            if (e.EventType == ZipProgressEventType.Saving_Completed)
            {
                Thread.Sleep(10);
                Directory.Delete(tempDir, true);
                ChangeStatus("Packing package... Done!");
                //ChangePercentage("100%");
                switch (MessageBox.Show("Packing successful! Operation complete!", "Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information))
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
    }
}
