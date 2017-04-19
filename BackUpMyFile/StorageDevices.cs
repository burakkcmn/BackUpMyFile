using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TimerHelp;

namespace BackUpMyFile
{
    public class StorageDevices : IDisposable
    {
        /// <summary>
        /// USAGE
        /// StorageDevices storageDevices = new StorageDevices();
        /// storageDevices.DriveMatched += new System.EventHandler((sender, e) => HandleStorageDevice(sender, e));
        /// private void HandleStorageDevice(object sender, EventArgs e) { }
        /// </summary>

        private DetectUSBDrive detectUSBdrive_;

        #region Event
        public event EventHandler DriveMatched;
        protected virtual void OnNewDriveMatched()
        {
            DriveMatched?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public StorageDevices(string[] _desiredDrives = null)
        {
            try
            {
                detectUSBdrive_ = new DetectUSBDrive(DeviceInsertedEvent, DeviceRemovedEvent);
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }
        
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            OnNewDriveMatched();
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            OnNewDriveMatched();
        }

        public void Dispose()
        {
            detectUSBdrive_.Dispose();
            DriveMatched = null;
        }
    }
}