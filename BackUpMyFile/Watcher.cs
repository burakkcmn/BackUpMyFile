using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpMyFile
{
    class Watcher : IDisposable
    {
        public struct watcherStrct
        {
            public FileSystemWatcher Watcher;
        }

        public List<watcherStrct> WatcherList = new List<watcherStrct>();

        #region Event
        /// <summary>
        /// WatchCaught += new System.EventHandler(delegate (object sender, EventArgs args)
        /// {
        /// HandleWatchCaught();
        /// });
        /// 
        /// private void HandleWatchCaught()
        /// {
        /// //...........
        /// }
        /// </summary>
        public event EventHandler WatchCaught;
        protected virtual void OnWatchcaught(string watchingPath)
        {
            WatchCaught?.Invoke(watchingPath, EventArgs.Empty);
        }
        #endregion

        public void AddWatch(params string[] paths)
        {
            foreach (var path in paths)
            {
                try
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(path);
                    watcher.Filter = "*.*";
                    watcher.IncludeSubdirectories = true;

                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                                           NotifyFilters.DirectoryName;

                    watcher.Changed += OnChanged;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnChanged;
                    //watcher.EnableRaisingEvents = true;

                    watcherStrct ws = new watcherStrct();
                    ws.Watcher = watcher;
                    if (!WatcherList.Contains(ws))
                        WatcherList.Add(ws);
                }
                catch (Exception ex)
                {
                    Tools.ErrorWriter(ex);
                    //return false;
                }
            }
            //return true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                int index = WatcherIndex(e.FullPath);
                if (index > -1)
                {
                    WatcherList[index].Watcher.EnableRaisingEvents = false;
                    if (WatchCaught != null)
                        OnWatchcaught(WatcherList.ElementAt(index).Watcher.Path);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }

        public int WatcherIndex(string path)
        {
            try
            {
                foreach (var watch in WatcherList)
                {
                    if (path.Contains(watch.Watcher.Path))
                    {
                        return WatcherList.IndexOf(watch);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
            return -1;
        }

        public bool Start(string watchingPath)
        {
            try
            {
                int index = WatcherIndex(watchingPath);
                WatcherList[index].Watcher.EnableRaisingEvents = true;
            }
            catch(Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        public bool Stop(string watchingPath)
        {
            try
            {
                int index = WatcherIndex(watchingPath);
                WatcherList[index].Watcher.EnableRaisingEvents = false;
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            WatcherList = null;
            WatchCaught = null;
        }
    }
}
