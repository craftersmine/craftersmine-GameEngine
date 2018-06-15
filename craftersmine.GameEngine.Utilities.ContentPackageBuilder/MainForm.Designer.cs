using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace craftersmine.GameEngine.Utilities.ContentPackageBuilder
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.listView1 = new ListView();
            this.imageList1 = new ImageList(this.components);
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.button4 = new Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(306, 416);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new Size(16, 16);
            this.imageList1.TransparentColor = Color.Transparent;
            // 
            // button1
            // 
            this.button1.Location = new Point(324, 12);
            this.button1.Name = "button1";
            this.button1.Size = new Size(161, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new Point(324, 41);
            this.button2.Name = "button2";
            this.button2.Size = new Size(161, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new Point(324, 108);
            this.button3.Name = "button3";
            this.button3.Size = new Size(161, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new Point(324, 181);
            this.button4.Name = "button4";
            this.button4.Size = new Size(161, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(497, 440);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView listView1;
        private ImageList imageList1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
    }
}