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
using TimerHelp;
using ProtoBuf;

namespace BackUpMyFile
{
    public partial class ArchiveForm : Form
    {
        #region Event
        /// <summary>
        /// ItemDeleted += new System.EventHandler((sender, e) => HandleItemDeleted(sender, e));
        /// 
        /// private void HandleItemDeleted(object sender, EventArgs e)
        /// {
        /// //...........
        /// }
        /// </summary>
        public event EventHandler ItemDeleted;
        protected virtual void OnItemDeleted(int index)
        {
            ItemDeleted?.Invoke(index, EventArgs.Empty);
        }

        public event EventHandler ItemSelected;
        protected virtual void OnItemSelected(string[] param)
        {
            ItemSelected?.Invoke(param, EventArgs.Empty);
        }
        #endregion

        [Serializable, ProtoContract(Name = @"ArchiveData")]
        public struct ArchiveData
        {
            [ProtoMember(1, IsRequired = false, OverwriteList = true, Name = @"SourceFolder", DataFormat = DataFormat.Default)]
            [DefaultValue("")]
            public string SourceFolder { get; set; }

            [ProtoMember(2, IsRequired = false, OverwriteList = true, Name = @"DestinationFolder", DataFormat = DataFormat.Default)]
            [DefaultValue("")]
            public string DestinationFolder { get; set; }

            [ProtoMember(3, IsRequired = false, OverwriteList = true, Name = @"BackUpPeriod", DataFormat = DataFormat.Default)]
            [DefaultValue(0)]
            public int BackUpPeriod { get; set; }

            public ArchiveData(string sfolder, string dfolder, int per)
            {
                SourceFolder = sfolder;
                DestinationFolder = dfolder;
                BackUpPeriod = per;
            }
        }

        private bool IsChanged = false;

        public ArchiveForm()
        {
            InitializeComponent();
            Inıt();
        }

        /// <summary>
        /// Load archive elements in form
        /// </summary>
        public void Inıt()
        {
            try
            {
                No.Items.Clear();
                Sources.Items.Clear();
                Destinations.Items.Clear();
                Period.Items.Clear();
                
                int i = 1;
                foreach (var archive in Form1.programData.Archive)
                {
                    No.Items.Add(i);
                    i++;
                    Sources.Items.Add(archive.SourceFolder);
                    Destinations.Items.Add(archive.DestinationFolder);
                    if (archive.BackUpPeriod == -1) Period.Items.Add("When Change"); else Period.Items.Add(archive.BackUpPeriod);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        private void Archive_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsChanged)
            {
                Tools.Update(Form1.programData);
                IsChanged = false;
            }
            ItemDeleted = null;
        }

        /// <summary>
        /// Delete archive element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dlt_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (No.Items.Count > 0 && Sources.Items.Count > 00 && Destinations.Items.Count > 0)
                {
                    int index = Convert.ToInt32(No.SelectedItem) - 1;
                    Form1.programData.Archive.RemoveAt(index);
                    IsChanged = true;
                    Inıt();
                    OnItemDeleted(index);
                }
            }
            catch(Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        private void select_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(No.SelectedItem) - 1;
            OnItemSelected(new string[] { Sources.Items[index].ToString(), Destinations.Items[index].ToString(), Period.Items[index].ToString() });
            this.Close();
        }

        private void No_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = No.SelectedIndex;
            Sources.SelectedIndex = index;
            Destinations.SelectedIndex = index;
            Period.SelectedIndex = index;
        }

        private void Sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Sources.SelectedIndex;
            No.SelectedIndex = index;
            Destinations.SelectedIndex = index;
            Period.SelectedIndex = index;
        }

        private void Destinations_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Destinations.SelectedIndex;
            No.SelectedIndex = index;
            Sources.SelectedIndex = index;
            Period.SelectedIndex = index;
        }

        private void Period_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Period.SelectedIndex;
            No.SelectedIndex = index;
            Sources.SelectedIndex = index;
            Destinations.SelectedIndex = index;
        }
    }
}
