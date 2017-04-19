namespace BackUpMyFile
{
    partial class Options
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
            this.autoBackup_checkBox = new System.Windows.Forms.CheckBox();
            this.RunWithWin_checkBoc = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // autoBackup_checkBox
            // 
            this.autoBackup_checkBox.AutoSize = true;
            this.autoBackup_checkBox.Location = new System.Drawing.Point(8, 35);
            this.autoBackup_checkBox.Name = "autoBackup_checkBox";
            this.autoBackup_checkBox.Size = new System.Drawing.Size(88, 17);
            this.autoBackup_checkBox.TabIndex = 3;
            this.autoBackup_checkBox.Text = "Auto Backup";
            this.autoBackup_checkBox.UseVisualStyleBackColor = true;
            this.autoBackup_checkBox.CheckedChanged += new System.EventHandler(this.autoBackup_checkBox_CheckedChanged);
            // 
            // RunWithWin_checkBoc
            // 
            this.RunWithWin_checkBoc.AutoSize = true;
            this.RunWithWin_checkBoc.Location = new System.Drawing.Point(8, 12);
            this.RunWithWin_checkBoc.Name = "RunWithWin_checkBoc";
            this.RunWithWin_checkBoc.Size = new System.Drawing.Size(85, 17);
            this.RunWithWin_checkBoc.TabIndex = 2;
            this.RunWithWin_checkBoc.Text = "Run StartUp";
            this.RunWithWin_checkBoc.UseVisualStyleBackColor = true;
            this.RunWithWin_checkBoc.CheckedChanged += new System.EventHandler(this.RunWithWin_checkBoc_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(49, 58);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Choice";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(99, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "fast(0)";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(141, 104);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.autoBackup_checkBox);
            this.Controls.Add(this.RunWithWin_checkBoc);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoBackup_checkBox;
        private System.Windows.Forms.CheckBox RunWithWin_checkBoc;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}