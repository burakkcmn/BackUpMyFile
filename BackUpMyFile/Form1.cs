using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TimerHelp;

namespace BackUpMyFile
{
    public partial class Form1 : Form
    {
        public static class BackupMethods
        {
            public const string Periodically = "Periodically";
            public const string Whenfilechanged = "When file changed";

            public static string[] GetArray()
            {
                var fields = typeof(BackupMethods).GetFields(BindingFlags.Public | BindingFlags.Static);
                string[] stringfields = new string[fields.Length];
                for (int i=0;i<fields.Length;i++)
                {
                    stringfields[i] = fields[i].GetValue(null).ToString();
                }
                return stringfields;
            }
        }
        private ArchiveForm archiveForm;
        private Options optionsForm;

        public static ProgramData programData;

        private Thread thrd;
        private AutoBackup autobackup;

        public Form1()
        {
            InitializeComponent();
            backupMethod.Items.AddRange(BackupMethods.GetArray());
            LoadFiles();

            programData = new ProgramData();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + Tools.probuffile))
                Tools.Refresh(ref programData);
            BackupPeriod_seconds.Maximum = decimal.MaxValue;
            pBar.ProgressBarValueChange += HandleProgressBarChanging;
            HandleStartWithWindowsChanged(null, null);
            HandleAutoBackupChanged(null, null);
        }

        [Conditional("DEBUG")]
        private void LoadFiles()
        {
            try
            {
                Tools.CheckFile(Tools.logFile, Tools.errFile);
                Tools tls = new Tools();
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        private void HandleProgressBarChanging(pBar.data data)
        {
            progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = data.pbarValue; }));
            using (Graphics gr = progressBar1.CreateGraphics())
            {
                gr.DrawString(data.pbarText,
                    SystemFonts.DefaultFont,    //The font we will draw it it (TextFont)
                    Brushes.Black,              //The brush we will use to draw it
                    new PointF(                 //Where we will draw it
                        progressBar1.Width / 2 - (gr.MeasureString(data.pbarText,
                        SystemFonts.DefaultFont).Width / 2.0F),
                        progressBar1.Height / 2 - (gr.MeasureString(data.pbarText,
                        SystemFonts.DefaultFont).Height / 2.0F)));
            }
        }

        private void HandleStartWithWindowsChanged(object sender, EventArgs e)
        {
            if (programData.Options.StartWithWindowsStartup)
                Startup.AddToReg(Assembly.GetEntryAssembly().Location);
            else
                Startup.RemoveFromReg(System.IO.Path.GetFileName(Assembly.GetEntryAssembly().Location));
        }

        private void HandleAutoBackupChanged(object sender, EventArgs e)
        {
            if (programData.Options.AutoBackup)
            {
                autobackup = new AutoBackup();
            }
            else
            {
                autobackup = null;
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void backupMyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            SelectNextControl(ActiveControl, true, true, true, true);
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            SelectNextControl(ActiveControl, true, true, true, true);
        }

        /// <summary>
        /// Opens folder browser to choose source folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void source_textBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Specify the source folder.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                source_textBox.Text = (fbd.SelectedPath);
            }
        }
        
        /// <summary>
        /// Opens folder browser to choose destination folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void destination_textBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Specify the destination folder.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                destination_textBox.Text = (fbd.SelectedPath);
            }
        }

        /// <summary>
        /// Opens options form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (optionsForm == null) 
            {
                optionsForm = new Options();
                optionsForm.FormClosing += (o, form) =>
                {
                    Show();
                    optionsForm = null;
                };
                Hide();
                optionsForm.StartWithWindowsChanged += new EventHandler((_sender, _e) => HandleStartWithWindowsChanged(_sender, _e));
                optionsForm.AutoBackupChanged += new EventHandler((_sender, _e) => HandleAutoBackupChanged(_sender, _e));
                optionsForm.Show();
            }
            else
            {
                optionsForm.Focus();
            }
        }

        /// <summary>
        /// Opens archive form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void archive_btn_Click(object sender, EventArgs e)
        {
            if (archiveForm == null) 
            {
                archiveForm = new ArchiveForm();
                archiveForm.FormClosing += (o, form) =>
                {
                    Show();
                    archiveForm = null;
                };
                Hide();
                archiveForm.ItemDeleted += new EventHandler((_sender, _e) => HandleItemDeleted(_sender, _e));
                archiveForm.ItemSelected += new EventHandler((_sender, _e) => HandleItemSelected(_sender, _e));
                archiveForm.Show();
            }
            else
            {
                archiveForm.Focus();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Add element for auto backup to archive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_btn_Click(object sender, EventArgs e)
        {
            if (!Tools.IsADirectoryPath(source_textBox.Text) || !Tools.IsADirectoryPath(destination_textBox.Text))
            {
                MessageBox.Show("Enter a valid directory path.");
            }
            ArchiveForm.ArchiveData arcData = new ArchiveForm.ArchiveData();
            arcData.SourceFolder = source_textBox.Text;
            arcData.DestinationFolder = destination_textBox.Text;
            if (backupMethod.SelectedItem.ToString() == "When file changed")
                arcData.BackUpPeriod = -1;
            else
                arcData.BackUpPeriod = Convert.ToUInt16(BackupPeriod_seconds.Value);
            int index;
            if ((index = Tools.Contains(arcData, false)) > -1)
            {
                if (Tools.Contains(arcData) > -1)
                {
                    MessageBox.Show("This paths are exists.");
                }else
                {
                    programData.Archive.RemoveAt(index);
                    programData.Archive.Add(arcData);
                    Tools.Update(programData);
                    if (autobackup != null)
                        autobackup.ItemChanged(index);
                    MessageBox.Show("Periode was changed.");
                }
            }else
            {
                programData.Archive.Add(arcData);
                Tools.Update(programData);
                MessageBox.Show("Added successfully.");
            }
        }

        /// <summary>
        /// Backup manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_bckp_Click(object sender, EventArgs e)
        {
            if (!Tools.IsADirectoryPath(source_textBox.Text) || !Tools.IsADirectoryPath(destination_textBox.Text))
            {
                MessageBox.Show("Enter a valid directory path.");
            }
            else
            {
                Tools.LogWriter("Manual Backup\n");
                bool result = false;
                thrd = new Thread(()=>
                {
                    result = FileOperation.SynchronizeFiles(source_textBox.Text, destination_textBox.Text, programData.Options.CopySpeed);
                }) { IsBackground = true };
                thrd.Start();
            }
        }

        private void BackupPeriod_seconds_ValueChanged(object sender, EventArgs e)
        {
            if (BackupPeriod_seconds.Value == 0 || BackupPeriod_seconds.Value == 1)
            {
                label2.Text = "minute";
            }
            else
            {
                label2.Text = "minutes";
            }
        }

        private void backupMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(backupMethod.SelectedItem.ToString())
            {
                case BackupMethods.Periodically:
                    BackupPeriod_seconds.Visible = true;
                    label2.Visible = true;
                    break;
                case BackupMethods.Whenfilechanged:
                    BackupPeriod_seconds.Visible = false;
                    label2.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void HandleItemDeleted(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                //iterate through
            }

            autobackup.ItemDeleted(sender);
        }

        public void HandleItemSelected(object sender, EventArgs e)
        {
            string[] param = (string[])sender;
            source_textBox.Text = param[0];
            destination_textBox.Text = param[1];
            int period = param[2] == "When Change" ? -1 : Convert.ToInt32(param[2]);
            if (period > -1)
            {
                backupMethod.SelectedItem = BackupMethods.Periodically;
                BackupPeriod_seconds.Value = period;
            }
            else
            {
                backupMethod.SelectedItem = BackupMethods.Whenfilechanged;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (autobackup != null)
                autobackup.Dispose();
            if (archiveForm != null)
                archiveForm = null;
            if (optionsForm != null)
                optionsForm = null;
            if (thrd != null)
                thrd = null;
            programData = null;
            e.Cancel = false;
        }
    }
}