namespace BackUpMyFile
{
    partial class ArchiveForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.select = new System.Windows.Forms.Button();
            this.Destinations = new System.Windows.Forms.ListBox();
            this.Sources = new System.Windows.Forms.ListBox();
            this.No = new System.Windows.Forms.ListBox();
            this.dlt_btn = new System.Windows.Forms.Button();
            this.Period = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.83923F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.16077F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Controls.Add(this.select, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Destinations, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Sources, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.No, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dlt_btn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Period, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(735, 137);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // select
            // 
            this.select.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.select.Location = new System.Drawing.Point(31, 110);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(58, 24);
            this.select.TabIndex = 4;
            this.select.Text = "Select";
            this.select.UseVisualStyleBackColor = true;
            this.select.Click += new System.EventHandler(this.select_Click);
            // 
            // Destinations
            // 
            this.Destinations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Destinations.FormattingEnabled = true;
            this.Destinations.Location = new System.Drawing.Point(338, 3);
            this.Destinations.MultiColumn = true;
            this.Destinations.Name = "Destinations";
            this.Destinations.Size = new System.Drawing.Size(303, 101);
            this.Destinations.TabIndex = 1;
            this.Destinations.SelectedIndexChanged += new System.EventHandler(this.Destinations_SelectedIndexChanged);
            // 
            // Sources
            // 
            this.Sources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Sources.FormattingEnabled = true;
            this.Sources.Location = new System.Drawing.Point(31, 3);
            this.Sources.Name = "Sources";
            this.Sources.Size = new System.Drawing.Size(301, 101);
            this.Sources.TabIndex = 0;
            this.Sources.SelectedIndexChanged += new System.EventHandler(this.Sources_SelectedIndexChanged);
            // 
            // No
            // 
            this.No.Dock = System.Windows.Forms.DockStyle.Fill;
            this.No.FormattingEnabled = true;
            this.No.Location = new System.Drawing.Point(3, 3);
            this.No.Name = "No";
            this.No.Size = new System.Drawing.Size(22, 101);
            this.No.TabIndex = 2;
            this.No.SelectedIndexChanged += new System.EventHandler(this.No_SelectedIndexChanged);
            // 
            // dlt_btn
            // 
            this.dlt_btn.Dock = System.Windows.Forms.DockStyle.Right;
            this.dlt_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dlt_btn.Location = new System.Drawing.Point(583, 110);
            this.dlt_btn.Name = "dlt_btn";
            this.dlt_btn.Size = new System.Drawing.Size(58, 24);
            this.dlt_btn.TabIndex = 3;
            this.dlt_btn.Text = "Delete";
            this.dlt_btn.UseVisualStyleBackColor = true;
            this.dlt_btn.Click += new System.EventHandler(this.dlt_btn_Click);
            // 
            // Period
            // 
            this.Period.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Period.FormattingEnabled = true;
            this.Period.Location = new System.Drawing.Point(647, 3);
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(85, 101);
            this.Period.TabIndex = 4;
            this.Period.SelectedIndexChanged += new System.EventHandler(this.Period_SelectedIndexChanged);
            // 
            // ArchiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 137);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "ArchiveForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Archive";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Archive_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox Sources;
        private System.Windows.Forms.ListBox No;
        private System.Windows.Forms.ListBox Period;
        private System.Windows.Forms.Button dlt_btn;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.ListBox Destinations;
    }
}