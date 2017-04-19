namespace BackUpMyFile
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.source_btn = new System.Windows.Forms.Label();
            this.destination_btn = new System.Windows.Forms.Label();
            this.source_textBox = new System.Windows.Forms.TextBox();
            this.destination_textBox = new System.Windows.Forms.TextBox();
            this.archive_btn = new System.Windows.Forms.Button();
            this.add_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.backupMyFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btn_bckp = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.BackupPeriod_seconds = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.backupMethod = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackupPeriod_seconds)).BeginInit();
            this.SuspendLayout();
            // 
            // source_btn
            // 
            this.source_btn.AutoSize = true;
            this.source_btn.Location = new System.Drawing.Point(12, 9);
            this.source_btn.Name = "source_btn";
            this.source_btn.Size = new System.Drawing.Size(65, 13);
            this.source_btn.TabIndex = 2;
            this.source_btn.Text = "Source       :";
            // 
            // destination_btn
            // 
            this.destination_btn.AutoSize = true;
            this.destination_btn.Location = new System.Drawing.Point(12, 35);
            this.destination_btn.Name = "destination_btn";
            this.destination_btn.Size = new System.Drawing.Size(66, 13);
            this.destination_btn.TabIndex = 3;
            this.destination_btn.Text = "Destination :";
            // 
            // source_textBox
            // 
            this.source_textBox.Location = new System.Drawing.Point(83, 6);
            this.source_textBox.Name = "source_textBox";
            this.source_textBox.Size = new System.Drawing.Size(158, 20);
            this.source_textBox.TabIndex = 5;
            this.source_textBox.Text = "Double click";
            this.source_textBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.source_textBox_MouseDoubleClick);
            // 
            // destination_textBox
            // 
            this.destination_textBox.Location = new System.Drawing.Point(83, 32);
            this.destination_textBox.Name = "destination_textBox";
            this.destination_textBox.Size = new System.Drawing.Size(158, 20);
            this.destination_textBox.TabIndex = 6;
            this.destination_textBox.Text = "Double click";
            this.destination_textBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.destination_textBox_MouseDoubleClick);
            // 
            // archive_btn
            // 
            this.archive_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.archive_btn.Location = new System.Drawing.Point(250, 58);
            this.archive_btn.Name = "archive_btn";
            this.archive_btn.Size = new System.Drawing.Size(58, 20);
            this.archive_btn.TabIndex = 7;
            this.archive_btn.Text = "Archive";
            this.archive_btn.UseVisualStyleBackColor = true;
            this.archive_btn.Click += new System.EventHandler(this.archive_btn_Click);
            // 
            // add_btn
            // 
            this.add_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.add_btn.Location = new System.Drawing.Point(250, 6);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(58, 20);
            this.add_btn.TabIndex = 8;
            this.add_btn.Text = "Add";
            this.add_btn.UseVisualStyleBackColor = true;
            this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backupMyFileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            // 
            // backupMyFileToolStripMenuItem
            // 
            this.backupMyFileToolStripMenuItem.Name = "backupMyFileToolStripMenuItem";
            this.backupMyFileToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.backupMyFileToolStripMenuItem.Text = "BackupMyFile";
            this.backupMyFileToolStripMenuItem.Click += new System.EventHandler(this.backupMyFileToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "BackupMyFile";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btn_bckp
            // 
            this.btn_bckp.Location = new System.Drawing.Point(250, 32);
            this.btn_bckp.Name = "btn_bckp";
            this.btn_bckp.Size = new System.Drawing.Size(58, 20);
            this.btn_bckp.TabIndex = 9;
            this.btn_bckp.Text = "Backup";
            this.btn_bckp.UseVisualStyleBackColor = true;
            this.btn_bckp.Click += new System.EventHandler(this.btn_bckp_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 84);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(296, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 10;
            // 
            // BackupPeriod_seconds
            // 
            this.BackupPeriod_seconds.Location = new System.Drawing.Point(135, 58);
            this.BackupPeriod_seconds.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.BackupPeriod_seconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BackupPeriod_seconds.Name = "BackupPeriod_seconds";
            this.BackupPeriod_seconds.Size = new System.Drawing.Size(58, 20);
            this.BackupPeriod_seconds.TabIndex = 11;
            this.BackupPeriod_seconds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BackupPeriod_seconds.Visible = false;
            this.BackupPeriod_seconds.ValueChanged += new System.EventHandler(this.BackupPeriod_seconds_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "minute";
            this.label2.Visible = false;
            // 
            // backupMethod
            // 
            this.backupMethod.FormattingEnabled = true;
            this.backupMethod.Location = new System.Drawing.Point(12, 58);
            this.backupMethod.Name = "backupMethod";
            this.backupMethod.Size = new System.Drawing.Size(117, 21);
            this.backupMethod.TabIndex = 14;
            this.backupMethod.Text = "Backup Method";
            this.backupMethod.SelectedIndexChanged += new System.EventHandler(this.backupMethod_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 112);
            this.Controls.Add(this.backupMethod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BackupPeriod_seconds);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_bckp);
            this.Controls.Add(this.add_btn);
            this.Controls.Add(this.archive_btn);
            this.Controls.Add(this.destination_textBox);
            this.Controls.Add(this.source_textBox);
            this.Controls.Add(this.destination_btn);
            this.Controls.Add(this.source_btn);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BackupMyFile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BackupPeriod_seconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label source_btn;
        private System.Windows.Forms.Label destination_btn;
        private System.Windows.Forms.TextBox source_textBox;
        private System.Windows.Forms.TextBox destination_textBox;
        private System.Windows.Forms.Button archive_btn;
        private System.Windows.Forms.Button add_btn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem backupMyFileToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btn_bckp;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.NumericUpDown BackupPeriod_seconds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox backupMethod;
    }
}

