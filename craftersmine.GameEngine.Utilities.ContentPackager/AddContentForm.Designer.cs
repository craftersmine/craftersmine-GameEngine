namespace craftersmine.GameEngine.Utilities.ContentPackager
{
    partial class AddContentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.tex = new System.Windows.Forms.RadioButton();
            this.anim = new System.Windows.Forms.RadioButton();
            this.animMd = new System.Windows.Forms.RadioButton();
            this.font = new System.Windows.Forms.RadioButton();
            this.aud = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.str = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select content type:";
            // 
            // tex
            // 
            this.tex.AutoSize = true;
            this.tex.Location = new System.Drawing.Point(15, 34);
            this.tex.Name = "tex";
            this.tex.Size = new System.Drawing.Size(61, 17);
            this.tex.TabIndex = 1;
            this.tex.TabStop = true;
            this.tex.Text = "Texture";
            this.tex.UseVisualStyleBackColor = true;
            this.tex.CheckedChanged += new System.EventHandler(this.tex_CheckedChanged);
            // 
            // anim
            // 
            this.anim.AutoSize = true;
            this.anim.Location = new System.Drawing.Point(15, 57);
            this.anim.Name = "anim";
            this.anim.Size = new System.Drawing.Size(71, 17);
            this.anim.TabIndex = 2;
            this.anim.TabStop = true;
            this.anim.Text = "Animation";
            this.anim.UseVisualStyleBackColor = true;
            this.anim.CheckedChanged += new System.EventHandler(this.anim_CheckedChanged);
            // 
            // animMd
            // 
            this.animMd.AutoSize = true;
            this.animMd.Location = new System.Drawing.Point(15, 80);
            this.animMd.Name = "animMd";
            this.animMd.Size = new System.Drawing.Size(119, 17);
            this.animMd.TabIndex = 3;
            this.animMd.TabStop = true;
            this.animMd.Text = "Animation Metadata";
            this.animMd.UseVisualStyleBackColor = true;
            this.animMd.CheckedChanged += new System.EventHandler(this.animMd_CheckedChanged);
            // 
            // font
            // 
            this.font.AutoSize = true;
            this.font.Location = new System.Drawing.Point(15, 103);
            this.font.Name = "font";
            this.font.Size = new System.Drawing.Size(46, 17);
            this.font.TabIndex = 4;
            this.font.TabStop = true;
            this.font.Text = "Font";
            this.font.UseVisualStyleBackColor = true;
            this.font.CheckedChanged += new System.EventHandler(this.font_CheckedChanged);
            // 
            // aud
            // 
            this.aud.AutoSize = true;
            this.aud.Location = new System.Drawing.Point(15, 126);
            this.aud.Name = "aud";
            this.aud.Size = new System.Drawing.Size(84, 17);
            this.aud.TabIndex = 5;
            this.aud.TabStop = true;
            this.aud.Text = "Wave Audio";
            this.aud.UseVisualStyleBackColor = true;
            this.aud.CheckedChanged += new System.EventHandler(this.aud_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(50, 195);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Next";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // str
            // 
            this.str.AutoSize = true;
            this.str.Location = new System.Drawing.Point(15, 149);
            this.str.Name = "str";
            this.str.Size = new System.Drawing.Size(57, 17);
            this.str.TabIndex = 8;
            this.str.TabStop = true;
            this.str.Text = "Strings";
            this.str.UseVisualStyleBackColor = true;
            this.str.CheckedChanged += new System.EventHandler(this.str_CheckedChanged);
            // 
            // AddContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 230);
            this.Controls.Add(this.str);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.aud);
            this.Controls.Add(this.font);
            this.Controls.Add(this.animMd);
            this.Controls.Add(this.anim);
            this.Controls.Add(this.tex);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddContentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add content...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton tex;
        private System.Windows.Forms.RadioButton anim;
        private System.Windows.Forms.RadioButton animMd;
        private System.Windows.Forms.RadioButton font;
        private System.Windows.Forms.RadioButton aud;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton str;
    }
}