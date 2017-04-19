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
using ProtoBuf;

namespace BackUpMyFile
{
    public partial class Options : Form
    {
        [Serializable, ProtoContract(Name = @"OptionData")]
        public struct OptionData
        {
            [ProtoMember(1, IsRequired = false,Name = @"StartWithWindowsStartup", DataFormat = DataFormat.Default)]
            //[DefaultValue(true)]
            public bool StartWithWindowsStartup { get; set; }

            [ProtoMember(2, IsRequired = false, Name = @"AutoBackup", DataFormat = DataFormat.Default)]
            //[DefaultValue(false)]
            public bool AutoBackup { get; set; }

            [ProtoMember(3, IsRequired = false, Name = @"CopySpeed", DataFormat = DataFormat.Default)]
            [DefaultValue(5)]
            public UInt16 CopySpeed { get; set; }
        }

        private bool IsChanged = false;
        private bool IsFormLoaded = false;

        #region Events
        public event EventHandler StartWithWindowsChanged;
        protected void OnStartWithWindowsChanged()
        {
            StartWithWindowsChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AutoBackupChanged;
        protected void OnAutoBackupChanged()
        {
            AutoBackupChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SpeedChanged;
        protected void OnSpeedChanged()
        {
            SpeedChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        public Options()
        {
            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        /// <summary>
        /// Load options in form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options_Load(object sender, EventArgs e)
        {
            autoBackup_checkBox.Checked = Form1.programData.Options.AutoBackup;
            RunWithWin_checkBoc.Checked = Form1.programData.Options.StartWithWindowsStartup;
            numericUpDown1.Value = Form1.programData.Options.CopySpeed;
            IsFormLoaded = true;
        }

        /// <summary>
        /// AutoBackup choise changed handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoBackup_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoBackup_checkBox.Checked)
            {
                autoBackup_checkBox.Text = "Auto Backup(E)";
            }
            else
            {
                autoBackup_checkBox.Text = "Auto Backup(D)";
            }

            Form1.programData.Options.AutoBackup = autoBackup_checkBox.Checked;
            OnAutoBackupChanged();
            if (IsFormLoaded) IsChanged = true;
        }

        /// <summary>
        /// RunWithWin choise changed handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunWithWin_checkBoc_CheckedChanged(object sender, EventArgs e)
        {
            if (RunWithWin_checkBoc.Checked)
            {
                RunWithWin_checkBoc.Text = "Run StartUp(E)";
            }
            else
            {
                RunWithWin_checkBoc.Text = "Run StartUp(D)";
            }

            Form1.programData.Options.StartWithWindowsStartup = RunWithWin_checkBoc.Checked;
            OnStartWithWindowsChanged();
            if (IsFormLoaded) IsChanged = true;
        }

        /// <summary>
        /// Speed variable changed handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Form1.programData.Options.CopySpeed = (UInt16)numericUpDown1.Value;
            OnSpeedChanged();
            if (IsFormLoaded) IsChanged = true;
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChanged)
            {
                Tools.Update(Form1.programData);
                IsChanged = false;
            }
            StartWithWindowsChanged = null;
            AutoBackupChanged = null;
            SpeedChanged = null;
        }
    }
}