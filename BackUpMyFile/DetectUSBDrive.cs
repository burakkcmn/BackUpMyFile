using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUpMyFile
{
    public class DetectUSBDrive : IDisposable
    {
        /// <summary>
        /// Watcher for inserted devices
        /// </summary>
        private ManagementEventWatcher insertWatcher;
        /// <summary>
        /// Watcher for removed devices
        /// </summary>
        private ManagementEventWatcher removeWatcher;

        /// <summary>
        /// Detect Usb devices that inserted or removed
        /// </summary>
        /// <param name="deviceInsertedEvent">EventHandler for inserted devices as EventArrivedEventHandler</param>
        /// <param name="deviceRemovedEvent">EventHandler for removed devices as EventArrivedEventHandler</param>
        public DetectUSBDrive(EventArrivedEventHandler deviceInsertedEvent, EventArrivedEventHandler deviceRemovedEvent)
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

            insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(deviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(deviceRemovedEvent);
            removeWatcher.Start();
        }

        public void Dispose()
        {
            insertWatcher.Dispose();
            removeWatcher.Dispose();
        }

        /*
        ///Example///
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            MessageBox.Show("Plugd in!");
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            MessageBox.Show("Plugd out!");
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }
        */
    }
}
