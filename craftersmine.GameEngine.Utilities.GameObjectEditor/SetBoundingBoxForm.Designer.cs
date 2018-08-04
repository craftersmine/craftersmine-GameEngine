namespace craftersmine.GE.Utilities.GameObjectEditor
{
    partial class SetBoundingBoxForm
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
            this.xoff = new System.Windows.Forms.NumericUpDown();
            this.yoff = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.bboxwidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.bboxheight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.xoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bboxwidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bboxheight)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X (Offset):";
            // 
            // xoff
            // 
            this.xoff.Location = new System.Drawing.Point(72, 7);
            this.xoff.Name = "xoff";
            this.xoff.Size = new System.Drawing.Size(178, 22);
            this.xoff.TabIndex = 1;
            // 
            // yoff
            // 
            this.yoff.Location = new System.Drawing.Point(72, 33);
            this.yoff.Name = "yoff";
            this.yoff.Size = new System.Drawing.Size(178, 22);
            this.yoff.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y (Offset):";
            // 
            // bboxwidth
            // 
            this.bboxwidth.Location = new System.Drawing.Point(72, 59);
            this.bboxwidth.Name = "bboxwidth";
            this.bboxwidth.Size = new System.Drawing.Size(178, 22);
            this.bboxwidth.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Width:";
            // 
            // bboxheight
            // 
            this.bboxheight.Location = new System.Drawing.Point(72, 85);
            this.bboxheight.Name = "bboxheight";
            this.bboxheight.Size = new System.Drawing.Size(178, 22);
            this.bboxheight.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Height:";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(175, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(94, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SetBoundingBoxForm
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(262, 145);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bboxheight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bboxwidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.yoff);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.xoff);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetBoundingBoxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set object bounding box";
            ((System.ComponentModel.ISupportInitialize)(this.xoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bboxwidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bboxheight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown xoff;
        private System.Windows.Forms.NumericUpDown yoff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown bboxwidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown bboxheight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}