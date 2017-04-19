using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimerHelp;

namespace BackUpMyFile
{
    public class AutoBackup : IDisposable
    {
        private class AutoBackupData
        {
            public string sourceFolder;
            public string sourceDriveLetter;
            public string destFolder;
            public string destinationDriveLetter;
            public int period;
            public MyTimer Timer;
            public bool isPeriodCompleted;
            public bool isDestinationDriveReady;
            public bool isSourceDriveReady;
            public bool isFileChanged;

            public AutoBackupData(ArchiveForm.ArchiveData archiveData, TimerCallback timerCallBack, EventHandler handleWatchCaught)
            {
                this.sourceFolder = archiveData.SourceFolder;
                this.sourceDriveLetter = Tools.GetDriveLetter(archiveData.SourceFolder);
                this.destFolder = archiveData.DestinationFolder;
                this.destinationDriveLetter = Tools.GetDriveLetter(archiveData.DestinationFolder);
                this.period = archiveData.BackUpPeriod;
                if (archiveData.BackUpPeriod == Method.Whenfilechanged)
                {
                    this.Timer = null;
                }
                else
                {
                    this.Timer = new MyTimer(timerCallBack, 0, period * 60000, archiveData.SourceFolder);
                }
                IsDriveReady();
            }

            private bool IsDriveReady()
            {
                DriveInfo[] dinfo = System.IO.DriveInfo.GetDrives();
                foreach (var drive in dinfo)
                {
                    if (drive.Name.Contains(destinationDriveLetter))
                    {
                        isDestinationDriveReady = true;
                    }
                    if(drive.Name.Contains(sourceDriveLetter))
                    {
                        isSourceDriveReady = true;
                    }
                }
                return false;
            }
        }
        
        private static class Method
        {
            public const int Whenfilechanged = -1;
        }

        /// <summary>
        /// Data to use AutoBackup class
        /// </summary>
        private List<AutoBackupData> autoBackupDatas;

        /// <summary>
        /// to Tracks USB devices
        /// </summary>
        private StorageDevices _storageDevices;
        
        private List<BackgroundWorker> workerList;
        private Watcher watcher_;

        public AutoBackup()
        {
            autoBackupDatas = new List<AutoBackupData>();

            _storageDevices = new StorageDevices();
            _storageDevices.DriveMatched += new System.EventHandler((sender, e) => HandleStorageDevice(sender, e));

            watcher_ = new Watcher();
            watcher_.WatchCaught += new System.EventHandler((sender, e) => HandleWatchCaught(sender, e));

            workerList = new List<BackgroundWorker>();

            HandleStorageDevice(null, null);

            LoadBackUpData();
        }

        /// <summary>
        /// Load and build BackUpDatas from archiveData struct
        /// </summary>
        private void LoadBackUpData()
        {
            try
            {
                foreach(var data in Form1.programData.Archive)
                {
                    ///Load BackupData from archiveData
                    autoBackupDatas.Add(new AutoBackupData(data, TimersCallBack, HandleWatchCaught));
                    if (data.BackUpPeriod == Method.Whenfilechanged)
                    {
                        watcher_.AddWatch(data.SourceFolder);
                        watcher_.Start(data.SourceFolder);
                    }
                }
                foreach (var autoBackupData in autoBackupDatas)
                {
                    AddWorker(autoBackupData);
                }
            }
            catch(Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        /// <summary>
        /// Add work in BackgroundWorker List
        /// </summary>
        /// <param name="autoBackupData"></param>
        private void AddWorker(AutoBackupData autoBackupData)
        {
            ///Build Workers///
            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += (obj, e) => { e.Result = FileOperation.SynchronizeFiles(autoBackupData.sourceFolder, autoBackupData.destFolder, Form1.programData.Options.CopySpeed); };
            BW.RunWorkerCompleted += (obj, e) => backgroundWorker1_RunWorkerCompleted(obj, e, autoBackupData);
            workerList.Add(BW);
            if (!workerList[0].IsBusy) { workerList[0].RunWorkerAsync(); }
        }

        /// <summary>
        /// Build BackgroundWorker to copy
        /// </summary>
        /// <param name="autoBackupData"></param>
        private void CopyIfReady(AutoBackupData autoBackupData)
        {
            try
            {
                if (autoBackupData.isDestinationDriveReady && autoBackupData.isSourceDriveReady &&
                    ((autoBackupData.period == Method.Whenfilechanged) ? autoBackupData.isFileChanged : autoBackupData.isPeriodCompleted))
                {
                    AddWorker(autoBackupData);
                }
                else Tools.LogWriter("It's not ready to copy : " + 
                    ((autoBackupData.period == Method.Whenfilechanged)
                    ? "(s-d-chng) " + autoBackupData.isSourceDriveReady + autoBackupData.isDestinationDriveReady + autoBackupData.isFileChanged : 
                    "(s-d-per) " + autoBackupData.isSourceDriveReady + autoBackupData.isDestinationDriveReady + autoBackupData.isPeriodCompleted));
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        /// <summary>
        /// Find element index in AutoBackupDatas List
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        private int AutoBackupDatasIndex(string sourcePath)
        {
            for (int i = 0; i < autoBackupDatas.Count; i++)
            {
                if (autoBackupDatas[i].sourceFolder == sourcePath)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Watcher event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleWatchCaught(object sender, EventArgs e)
        {
            string watchingPath = sender.ToString();
            int index = AutoBackupDatasIndex(watchingPath);
            if (index > -1)
            {
                Tools.LogWriter(watchingPath + " is changed. isFileChanged = true, EnableRaisingEvents = false");
                autoBackupDatas[index].isFileChanged = true;
                watcher_.Stop(autoBackupDatas[index].sourceFolder);
                CopyIfReady(autoBackupDatas[index]);
            }
        }

        /// <summary>
        /// AutoBackUp period's timer
        /// </summary>
        /// <param name="state"></param>
        private void TimersCallBack(object state)
        {
            if (state != null)
            {
                string Path = state as string;
                int index = AutoBackupDatasIndex(Path);
                if (index > -1)
                {
                    Tools.LogWriter("Time's up for " + autoBackupDatas[index].sourceFolder + ". isPeriodCompleted = true  (" + DateTime.Now + ")");
                    autoBackupDatas[index].isPeriodCompleted = true;
                    autoBackupDatas[index].Timer.Stop();
                    CopyIfReady(autoBackupDatas[index]);
                }
            }
        }

        /// <summary>
        /// Storage devices(USB) event(insert - remove)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleStorageDevice(object sender, EventArgs e)
        {
            try
            {
                DriveInfo[] dinfo = DriveInfo.GetDrives();
                for (int i = 0; i < autoBackupDatas.Count; i++)
                {
                    bool foundS = false;
                    bool foundD = false;
                    foreach (var di in dinfo)
                    {
                        if (di.Name.Contains(autoBackupDatas[i].sourceDriveLetter) && di.Name.Contains(autoBackupDatas[i].destinationDriveLetter))
                        {
                            Tools.LogWriter(di.Name + " insert");
                            autoBackupDatas[i].isSourceDriveReady = true;
                            autoBackupDatas[i].isDestinationDriveReady = true;
                            CopyIfReady(autoBackupDatas[i]);
                            foundS = true;
                            foundD = true;
                        }
                        else if (di.Name.Contains(autoBackupDatas[i].sourceDriveLetter))
                        {
                            Tools.LogWriter(di.Name + " insert");
                            autoBackupDatas[i].isSourceDriveReady = true;
                            CopyIfReady(autoBackupDatas[i]);
                            foundS = true;
                        }
                        else if (di.Name.Contains(autoBackupDatas[i].destinationDriveLetter))
                        {
                            Tools.LogWriter(di.Name + " insert");
                            autoBackupDatas[i].isDestinationDriveReady = true;
                            CopyIfReady(autoBackupDatas[i]);
                            foundD = true;
                        }
                    }
                    if (!foundS && !foundD)
                    {
                        autoBackupDatas[i].isDestinationDriveReady = false;
                        autoBackupDatas[i].isSourceDriveReady = false;
                        Tools.LogWriter(autoBackupDatas[i].isSourceDriveReady + " eject");
                    }
                    else if (!foundS)
                    {
                        autoBackupDatas[i].isSourceDriveReady = false;
                        Tools.LogWriter(autoBackupDatas[i].isSourceDriveReady + " eject");
                    }
                    else if (!foundD)
                    {
                        autoBackupDatas[i].isDestinationDriveReady = false;
                        Tools.LogWriter(autoBackupDatas[i].isDestinationDriveReady + " eject");
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        /// <summary>
        /// Event for deleted archive's element
        /// </summary>
        /// <param name="o">Element index</param>
        public void ItemDeleted(object o)
        {
            try
            {
                int index = (int)o;
                if (index > -1)
                {
                    Tools.LogWriter("Auto Backup item was deleted : " + autoBackupDatas[index].sourceFolder);
                    autoBackupDatas.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        /// <summary>
        /// Event for changed archive's element
        /// </summary>
        /// <param name="o">Element index</param>
        public void ItemChanged(int index)
        {
            try
            {
                if (index > -1)
                {
                    Tools.LogWriter("Auto Backup item was deleted : " + autoBackupDatas[index].sourceFolder);
                    autoBackupDatas.Add(new AutoBackupData(new ArchiveForm.ArchiveData(autoBackupDatas[index].sourceFolder, autoBackupDatas[index].destFolder, autoBackupDatas[index].period), TimersCallBack, HandleWatchCaught));
                    autoBackupDatas.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        /// <summary>
        /// Event for completed background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Uses for result</param>
        /// <param name="data">Work that is completed</param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e, AutoBackupData data)
        {
            try
            {
                if ((bool)e.Result == true)
                {
                    int index = autoBackupDatas.FindIndex(k => k == data);
                    Tools.LogWriter("------------------------------Worker completed. " + autoBackupDatas[index].sourceFolder + " remove from worker");
                    workerList.RemoveAt(0);
                    if (index > -1)
                    {
                        if (data.period == Method.Whenfilechanged)
                        {
                            Tools.LogWriter("Set values of isFileChanged to false", Tools.CreateFolderName(data.sourceFolder));
                            autoBackupDatas[index].isFileChanged = false;
                            watcher_.Start(data.sourceFolder);
                        }
                        else
                        {
                            Tools.LogWriter("Set values of isPeriodCompleted to false (" + DateTime.Now + ")");
                            autoBackupDatas[index].isPeriodCompleted = false;
                            autoBackupDatas[index].Timer.Start();
                        }
                    }
                }
                else
                {
                    Tools.LogWriter("Worker compleated but file senkronisation fail. worker move to end of list.", Tools.CreateFolderName(data.sourceFolder));
                    BackgroundWorker temp = workerList[0];
                    workerList.RemoveAt(0);
                    workerList.Add(temp);
                }

                if (workerList.Count > 0)
                {
                    workerList[0].RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        public void Dispose()
        {
            autoBackupDatas = null;
            _storageDevices.Dispose();
            if (workerList.Count < 0)
                if (workerList[0].IsBusy)
                    workerList[0].CancelAsync();
            workerList = null;

        }
    }
}
