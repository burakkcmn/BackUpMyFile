using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using ProtoBuf;

namespace BackUpMyFile
{
    internal static class pBar
    {
        /// <summary>
        /// ProgressBar variables that we will control it
        /// </summary>
        public struct data
        {
            public string pbarText;
            public int pbarValue;
            public data(int pbarvalue, string pbartext =null)
            {
                pbarText = pbartext;
                pbarValue = pbarvalue;
            }
        }

        /// <summary>
        /// ProgressBar value
        /// </summary>
        private static int _progressbarValue;
        public static int ProgressbarValue
        {
            get { return _progressbarValue; }
            set { _progressbarValue = value; OnProgressBarValueChange(new data(value)); }
        }

        /// <summary>
        /// ProgressBar text
        /// </summary>
        private static string _progressbarText;
        public static string ProgressbarText
        {
            get { return _progressbarText; }
            set { _progressbarText = value; }
        }

        /// <summary>
        /// Event for ProgressBar value change
        /// </summary>
        public static event Action<data> ProgressBarValueChange;

        /// <summary>
        /// Event trigger for ProgressBar value change
        /// </summary>
        /// <param name="progressbarData">ProgressBar values struct</param>
        private static void OnProgressBarValueChange(data progressbarData)
        {
            ProgressBarValueChange?.Invoke(progressbarData);
        }
    }

    internal class FileOperation
    {
        /// <summary>
        /// Stores file and folder paths
        /// </summary>
        public class FoldersAndFiles
        {
            /// <summary>
            /// Paths to folders
            /// </summary>
            private List<string> _key { get; set; }

            /// <summary>
            /// Paths to files. Parallel to key(folder list)
            /// </summary>
            private List<List<string>> _value { get; set; }
            
            private uint _folderCount { get; set; }
            
            private ulong _fileCount { get; set; }

            public FoldersAndFiles()
            {
                _key = new List<string>();
                _value = new List<List<string>>();
                _folderCount = 0;
                _fileCount = 0;
            }

            public List<string> Key
            {
                get { return _key; }
                set { _key = value; }
            }

            public List<List<string>> Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public uint FolderCount
            {
                get { return _folderCount; }
                set { _folderCount = value; }
            }

            public ulong FileCount
            {
                get { return _fileCount; }
                set { _fileCount = value; }
            }

            /// <summary>
            /// Adds a single folder and the files contained within it.
            /// </summary>
            /// <param name="key">Path of folder.</param>
            /// <param name="value">Paths list of files.</param>
            public void Add(string key, List<string> value)
            {
                if (key != null && value != null)
                {
                    Key.Add(key);
                    Value.Add(value);
                    FolderCount++;
                    FileCount += (ulong)value.Count;
                }
            }

            /// <summary>
            /// Get list of files by folder index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public List<string> GetValue(int index)
            {
                if (index >= 0)
                {
                    return Value[index];
                }
                return null;
            }

            /// <summary>
            /// Get list of files by folder path
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public List<string> GetValue(string key)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    int index = Key.FindIndex(k => k == key);
                    return Value[index];
                }
                return null;
            }

            /// <summary>
            /// Get folders list.
            /// </summary>
            /// <returns></returns>
            public List<string> GetKeys()
            {
                return Key;
            }

            /// <summary>
            /// Remove folder by folder index
            /// </summary>
            /// <param name="index"></param>
            public void Remove(int index)
            {
                if (index >= 0)
                {
                    FolderCount--;
                    Key.RemoveAt(index);
                    FileCount -= (ulong)Value[index].Count;
                    Key.RemoveAt(index);
                }
            }

            /// <summary>
            /// Remove folder by folder path
            /// </summary>
            /// <param name="key"></param>
            public void Remove(string key)
            {
                if (!string.IsNullOrEmpty((key)))
                {
                    int index = Key.FindIndex(k => k == key);
                    FolderCount--;
                    Key.RemoveAt(index);
                    FileCount -= (ulong)Value[index].Count;
                    Value.RemoveAt(index);
                }
            }

            /// <summary>
            /// Remove a file in file list by file and folder path
            /// </summary>
            /// <param name="key">Folder path</param>
            /// <param name="valueInValue">File path</param>
            public void RemoveValueInsideValue(string key, string valueInValue)
            {
                try
                {
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(valueInValue))
                    {
                        int index = Key.FindIndex(k => k == key);
                        Value[index].Remove(valueInValue);
                        FileCount--;
                    }
                }
                catch (Exception ex)
                {
                    Tools.ErrorWriter(ex);
                }
            }

            public void Dispose()
            {
                Key.Clear();
                Value.Clear();
                FolderCount = 0;
                FileCount = 0;
            }
        }
        
        /// <summary>
        /// Files list of failed copy
        /// </summary>
        [Serializable, ProtoContract(Name = @"FailedFiles", IgnoreListHandling = true)]
        public struct FailedFiles
        {
            [ProtoMember(1, IsRequired = false, Name = @"Files", DataFormat = DataFormat.Default)]
            [DefaultValue(default(List<string>))]
            public List<string> Files;

            public FailedFiles(List<List<string>> files)
            {
                Files = new List<string>();
            }
        }

        /// <summary>
        /// Copy single file from source to destination.
        /// </summary>
        /// <param name="sourcePath">Source file path</param>
        /// <param name="destinationPath">Destination file path</param>
        /// <returns></returns>
        private static bool CopySingleFile(string sourcePath, string destinationPath)
        {
            try
            {
                File.Copy(sourcePath, destinationPath);
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Copy files from source to destination
        /// </summary>
        /// <param name="sFoldersAndFiles">FoldersAndFiles object that contains file to copy</param>
        /// <param name="sourceMainPath">Top file path of source files</param>
        /// <param name="destinationMainPath">Top file path of destination files</param>
        /// <returns>True, if all the files are successfully copied.</returns>
        private static bool CopyFiles(ref FoldersAndFiles sFoldersAndFiles, string sourceMainPath,
            string destinationMainPath, int delay = 0)
        {
            try
            {
                ulong processedFileCount = 0;
                ulong totalFileCount = sFoldersAndFiles.FileCount;
                for (int j = 0; j < sFoldersAndFiles.GetKeys().Count; j++)
                {
                    string sFolder = sFoldersAndFiles.GetKeys()[j];
                    for (int i = 0; i < sFoldersAndFiles.GetValue(sFolder).Count; i++)
                    {
                        string sourcePath = sFoldersAndFiles.GetValue(sFolder)[i];
                        string newPath;
                        if (!string.IsNullOrEmpty(newPath = CreateNewPath(sourcePath, sourceMainPath, destinationMainPath)))
                        {
                            if (CopySingleFile(sourcePath, newPath))
                            {
                                sFoldersAndFiles.RemoveValueInsideValue(sFolder, sourcePath);
                                i--;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        processedFileCount++;
                    }
                    if (sFoldersAndFiles.GetValue(sFolder).Count == 0)
                    {
                        sFoldersAndFiles.Remove(sFolder);
                        j--;
                    }
                    pBar.ProgressbarValue = (int)(100 * processedFileCount / totalFileCount);
                    System.Threading.Thread.Sleep(delay);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>true, if it exists.</returns>
        public static bool IsFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Create a folder.
        /// </summary>
        /// <param name="folderPath">Folder path with name</param>
        /// <returns>true, if it was created</returns>
        public static bool CreateFolder(string folderPath)
        {
            try
            {
                if (!IsFolderExists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Find path between main folder and file/folder 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mainPath"></param>
        /// <returns></returns>
        private static string SubPath(string path, string mainPath)
        {
            try
            {
                if (!(string.IsNullOrEmpty(path) &&
                      string.IsNullOrEmpty(mainPath)))
                {
                    int index = path.IndexOf(mainPath);
                    return path.Remove(index, mainPath.Length);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Find path between main folder and file/folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mainPath"></param>
        /// <returns></returns>
        private static string UpperPath(string path, string mainPath)
        {
            try
            {
                if (!(string.IsNullOrEmpty(path) &&
                      string.IsNullOrEmpty(mainPath)))
                {
                    int index = path.IndexOf(mainPath);
                    int sind = path.Length;
                    int dind = mainPath.Length;
                    return path.Length > mainPath.Length ? path.Remove(0, mainPath.Length + 1) : null;
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Create a path for destination according to source.
        /// </summary>
        /// <param name="source">Path of source.</param>
        /// <param name="sourceMainPath">Main path of source.</param>
        /// <param name="destinationMainPath">Main path of destination.</param>
        /// <returns></returns>
        private static string CreateNewPath(string source, string sourceMainPath, string destinationMainPath)
        {
            try
            {
                if (!(string.IsNullOrEmpty(source) &&
                      string.IsNullOrEmpty(sourceMainPath) &&
                      string.IsNullOrEmpty(destinationMainPath)))
                {
                    string subPath;
                    if (!string.IsNullOrEmpty(subPath = SubPath(source, sourceMainPath)))
                    {
                        return destinationMainPath + subPath;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Create all folders that gives parameter
        /// </summary>
        /// <param name="foldersPath">List of folders path.</param>
        /// <param name="sourceMainPath">Main path of source.</param>
        /// <param name="destinationMainPath">Main path of destination.</param>
        /// <returns></returns>
        private static bool CreateFolders(List<string> foldersPath, string sourceMainPath, string destinationMainPath)
        {
            try
            {
                foldersPath.Sort();
                foreach (string folderPath in foldersPath)
                {
                    string newPath;
                    if (!string.IsNullOrEmpty(newPath = CreateNewPath(folderPath, sourceMainPath, destinationMainPath)))
                    {
                        if (!CreateFolder(newPath))
                        {
                            Tools.LogWriter("******************" + newPath + " not created!!!!!!!!!!!!!!!!!!!!!!!!", Tools.CreateFolderName(sourceMainPath));
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete single folder that gives in parameter
        /// </summary>
        /// <param name="folder">Folder that will delete</param>
        /// <returns></returns>
        private static bool DeleteFolder(string folder)
        {
            bool flag1 = false;
            try
            {
                Directory.Delete(folder, true);
            }
            catch (UnauthorizedAccessException)
            {
                flag1 = true;
            }
            try
            {
                if (flag1)
                {
                    //Directory.SetAccessControl(folder, FileAttributes.Normal);
                    //Directory.Delete(folder, true);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete single file that gives in parameter
        /// </summary>
        /// <param name="file">File that will delete</param>
        /// <returns></returns>
        private static bool DeleteFile(string file)
        {
            bool flag1 = false;
            try
            {
                File.Delete(file);
            }
            catch (UnauthorizedAccessException)
            {
                flag1 = true;
            }
            try
            {
                if (flag1)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete all files that gives in parameter
        /// </summary>
        /// <param name="files">Files that will delete</param>
        /// <returns></returns>
        private static bool DeleteFiles(List<string> files)
        {
            try
            {
                foreach (var file in files)
                {
                    DeleteFile(file);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compare lastwritetime of file/folder path
        /// </summary>
        /// <param name="sPath">Path of source</param>
        /// <param name="dPath">Path of destination</param>
        /// <returns></returns>
        private static bool CompareLastWriteTime(DateTime sPath, DateTime dPath)
        {
            if (dPath == sPath)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Comparation two path
        /// </summary>
        /// <param name="sPath">Path of source</param>
        /// <param name="dPath">Path of destination</param>
        /// <param name="sMainPath">Main path of source</param>
        /// <param name="dMainPath">Main path of destination</param>
        /// <returns></returns>
        private static bool CompareNames(string sPath, string sMainPath, string dPath, string dMainPath)
        {
            string _sPath = UpperPath(sPath, sMainPath);
            string _dPath = UpperPath(dPath, dMainPath);
            if (_sPath == _dPath)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Create folder and copy files
        /// </summary>
        /// <param name="sFoldersAndFiles">Source folders/files list</param>
        /// <param name="dFoldersAndFiles">Destination folders/files list</param>
        /// <returns></returns>
        private static bool CreateFoldersAndCopyFiles(ref FoldersAndFiles sFoldersAndFiles,
            ref FoldersAndFiles dFoldersAndFiles)
        {
            try
            {
                List<string> folderWillCreate = new List<string>(sFoldersAndFiles.GetKeys());

            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Comparison for single file
        /// </summary>
        /// <param name="sPath">Path of source file.</param>
        /// <param name="sMainPath">Main path of source.</param>
        /// <param name="dPath">Path of destination file.</param>
        /// <param name="dMainPath">Main path of destination.</param>
        /// <returns></returns>
        private static bool CompareSingleFile(string sPath, string sMainPath, string dPath, string dMainPath)
        {
            if (CompareNames(sPath, sMainPath, dPath, dMainPath))
            {
                FileInfo sfi = new FileInfo(sPath);
                FileInfo dfi = new FileInfo(dPath);
                if (CompareLastWriteTime(sfi.LastWriteTime, dfi.LastWriteTime))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Comparison for source and target files
        /// </summary>
        /// <param name="sFolder">Path of source folder that contain files.</param>
        /// <param name="sMainPath">Main path of source.</param>
        /// <param name="dFolder">Path of destination folder that contain files.</param>
        /// <param name="dMainPath">Main path of destination.</param>
        /// <param name="sFoldersAndFiles">Class that contain source folders/files list. After comparation, this class will return with the changed files.</param>
        /// <param name="dFoldersAndFiles">Class that contain destination folders/files list. After comparation, this class will return with the unchanged files</param>
        /// <returns></returns>
        private static bool CompareFiles(string sFolder, string sMainPath, string dFolder, string dMainPath, ref FoldersAndFiles sFoldersAndFiles,
            ref FoldersAndFiles dFoldersAndFiles)
        {
            try
            {
                List<string> liste = new List<string>();
                for (int i = 0; i < dFoldersAndFiles.GetValue(dFolder).Count; i++)    //Hedef klasör içindeki dosyaları tek tek getir
                {
                    bool isFileFoundAfterComparation = false;
                    string dFile = dFoldersAndFiles.GetValue(dFolder)[i];
                    for (int j = 0; j < sFoldersAndFiles.GetValue(sFolder).Count; j++)  // Kaynak klasör içindeki dosyaları tek tek getir.
                    {
                        string sFile = sFoldersAndFiles.GetValue(sFolder)[j];
                        if (CompareSingleFile(sFile, sMainPath, dFile, dMainPath))
                        {
                            #region DEBUGMESSAGE
#if DEBUG
                            //Tools.LogWriter("Matched : " + sFile + " and " + dFile, Tools.CreateFolderName(sMainPath));
#endif
                            #endregion
                            isFileFoundAfterComparation = true;
                            sFoldersAndFiles.RemoveValueInsideValue(sFolder, sFile);
                            j--;
                            dFoldersAndFiles.RemoveValueInsideValue(dFolder, dFile);
                            i--;
                            break;
                        }
                    }
                    if (!isFileFoundAfterComparation)
                    {
                        #region DEBUGMESSAGE
#if DEBUG
                        Tools.LogWriter("UnMatched : " + dFile, Tools.CreateFolderName(sMainPath));
#endif
                        #endregion
                        DeleteFile(dFile);
                        dFoldersAndFiles.RemoveValueInsideValue(dFolder, dFile);
                        i--;
                        isFileFoundAfterComparation = false;
                    }
                }
                if (sFoldersAndFiles.GetValue(sFolder).Count <= 0)
                {
                    sFoldersAndFiles.Remove(sFolder);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Compare two folder path.
        /// </summary>
        /// <param name="sPath">Path of source.</param>
        /// <param name="sMainPath">Main path of source.</param>
        /// <param name="dPath">Path of destination.</param>
        /// <param name="dMainPath">Main path of destination.</param>
        /// <returns></returns>
        private static bool CompareSingleFolder(string sPath, string sMainPath, string dPath, string dMainPath)
        {
            if (CompareNames(sPath, sMainPath, dPath, dMainPath))
            {
                return true;

            }
            return false;
        }

        /// <summary>
        /// Comparison for source and target
        /// </summary>
        /// <param name="sFoldersAndFiles">Class that contain source folders/files list. After comparation, this class will return with the changed folders/files.</param>
        /// <param name="dFoldersAndFiles">Class that contain destination folders/files list. After comparation, this class will return with the unchanged folders/files.</param>
        /// <param name="sMainPath">Main path of source.</param>
        /// <param name="dMainPath">Main path of destination.</param>
        /// <returns></returns>
        private static bool CompareSourceAndDestination(ref FoldersAndFiles sFoldersAndFiles,
            ref FoldersAndFiles dFoldersAndFiles, string sMainPath, string dMainPath, int delay = 0)
        {
            try
            {
                long totalCount = dFoldersAndFiles.FolderCount;
                long processedFolderCount = 0;
                for (int i = 0; i < dFoldersAndFiles.GetKeys().Count; i++) //Hedef klasörleri teker teker getir
                {
                    bool isFolderFoundAfterComparation = false;
                    string dFolder = dFoldersAndFiles.GetKeys()[i];
                    foreach (var sFolder in sFoldersAndFiles.GetKeys())  //Kaynak klasörleri teker teker getir
                    {
                        if (CompareSingleFolder(sFolder, sMainPath, dFolder, dMainPath))
                        {
                            #region DEBUGMESSAGE
#if DEBUG
                            //Tools.LogWriter("Matched : " + sFolder + " and " + dFolder, Tools.CreateFolderName(sMainPath));
#endif
                            #endregion
                            isFolderFoundAfterComparation = true;
                            if (!CompareFiles(sFolder, sMainPath, dFolder, dMainPath, ref sFoldersAndFiles, ref dFoldersAndFiles))
                            {
                                return false;
                            }
                            break;
                        }
                    }
                    if (!isFolderFoundAfterComparation)
                    {
                        #region DEBUGMESSAGE
#if DEBUG
                        Tools.LogWriter("UnMatched : " + dFolder, Tools.CreateFolderName(sMainPath));
#endif
                        #endregion
                        DeleteFolder(dFolder);
                        dFoldersAndFiles.Remove(dFolder);
                        i--;
                        isFolderFoundAfterComparation = false;
                    }
                    pBar.ProgressbarValue = (int)(100 * (++processedFolderCount) / totalCount);
                    System.Threading.Thread.Sleep(delay);
                }
            }
            catch(DirectoryNotFoundException directoryNotFoundException)
            {
                Tools.LogWriter("Exception occur : " + directoryNotFoundException.StackTrace, Tools.CreateFolderName(sMainPath));
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// List all subfolders in folder
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <param name="allFolders">Subfolders list to return</param>
        /// <returns></returns>
        private static bool ListAllFolder(string path, ref List<string> allFolders)
        {
            allFolders.Add(path);
            try
            {
                allFolders.AddRange(Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Access denide. I can't access a file in this location.\nProbably this is a system file!");
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// List files in single folder
        /// </summary>
        /// <param name="folder">Folder path</param>
        /// <param name="allFiles">File list to return</param>
        /// <returns></returns>
        private static bool ListFilesInFolder(string folder, ref List<string> allFiles)
        {
            try
            {
                allFiles.AddRange(Directory.GetFiles(folder));
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// List all files in all folders
        /// </summary>
        /// <param name="Sourcemainfolder">Main folder of source</param>
        /// <param name="foldersAndFiles">Return that class object for folders and files</param>
        /// <param name="delay">Delay time as milliseconds</param>
        /// <returns></returns>
        private static bool ListEveryFileForEachFolder(string Sourcemainfolder, ref FoldersAndFiles foldersAndFiles,
            int delay = 0)
        {
            try
            {
                var folders = new List<string>();
                if (!ListAllFolder(Sourcemainfolder, ref folders)) return false;
                foreach (string folder in folders)
                {
                    List<string> allfiles = new List<string>();
                    if (ListFilesInFolder(folder, ref allfiles))
                    {
                        foldersAndFiles.Add(folder, allfiles);
                    }
                    else
                    {
                        MessageBox.Show("Can't get files.");
                        return false;
                    }
                    System.Threading.Thread.Sleep(delay);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create path of destination according to source file
        /// </summary>
        /// <param name="sourcePath">Path of source</param>
        /// <param name="destinationPath">Path of destination</param>
        /// <returns></returns>
        private static string CreateDestinationFolder(string sourcePath, string destinationPath)
        {
            List<string> splitedsourcepath = Tools.SplitPath(sourcePath);
            List<string> spliteddestinationpath = Tools.SplitPath(destinationPath);
            if (splitedsourcepath[splitedsourcepath.Count - 1] != spliteddestinationpath[spliteddestinationpath.Count - 1])
            {
                destinationPath = destinationPath + "\\" + splitedsourcepath[splitedsourcepath.Count - 1];
            }

            if (!IsFolderExists(destinationPath))
                CreateFolder(destinationPath);
            
            return destinationPath;
        }

        /// <summary>
        /// Match source and destination files than copy.
        /// </summary>
        /// <param name="source">Source path</param>
        /// <param name="destination">Destination path</param>
        /// <param name="delay">Synchronization speed</param>
        /// <returns></returns>
        public static bool SynchronizeFiles(string source, string destination, int delay)
        {
            try
            {
                #region DEBUGMESSAGE
#if DEBUG
                Tools.LogWriter("Preparing files.", Tools.CreateFolderName(source));
#endif
                #endregion
                destination = CreateDestinationFolder(source, destination);

                FoldersAndFiles sFoldersAndFiles = new FoldersAndFiles();
                FoldersAndFiles dFoldersAndFiles = new FoldersAndFiles();

                if (!ListEveryFileForEachFolder(source, ref sFoldersAndFiles, delay))
                {
                    #region DEBUG  //Write fail
#if DEBUG
                    Tools.LogWriter("List source folders/files method fail.", Tools.CreateFolderName(source));
                    Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
                    #endregion
                    return false;
                }
                if (!ListEveryFileForEachFolder(destination, ref dFoldersAndFiles, delay))
                {
                    #region DEBUG  //Write fail
#if DEBUG
                    Tools.LogWriter("List destination folders/files method fail.", Tools.CreateFolderName(source));
                    Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
                    #endregion
                    return false;
                }

                #region DEBUG  //Write folder and files count
#if DEBUG
                Tools.LogWriter("source : " + sFoldersAndFiles.FolderCount + " folder\t" + sFoldersAndFiles.FileCount + " files", Tools.CreateFolderName(source));
                Tools.LogWriter("destin : " + dFoldersAndFiles.FolderCount + " folder\t" + dFoldersAndFiles.FileCount + " files", Tools.CreateFolderName(source));
#endif
                #endregion

                if (!CompareSourceAndDestination(ref sFoldersAndFiles, ref dFoldersAndFiles, source, destination, delay))
                {
                    #region DEBUG  //Write fail
#if DEBUG
                    Tools.LogWriter("CompareSourceAndDestination method fail.", Tools.CreateFolderName(source));
                    Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
                    #endregion
                    return false;
                }
                dFoldersAndFiles.Dispose();

                #region DEBUG  //Write matched and Unmatched files list
#if DEBUG
                if (sFoldersAndFiles.FileCount > 0 || sFoldersAndFiles.FolderCount > 0)
                {
                    Tools.LogWriter("***************   Folders were not matched   *************", Tools.CreateFolderName(source));
                    Tools.LogWriter("This files will change :  ***", Tools.CreateFolderName(source));
                    foreach (var folder in sFoldersAndFiles.Key)
                    {
                        Tools.LogWriter("\t" + folder, Tools.CreateFolderName(source));
                        foreach (var file in sFoldersAndFiles.GetValue(folder))
                        {
                            Tools.LogWriter("\t\t" + file, Tools.CreateFolderName(source));
                        }
                    }
                    Tools.LogWriter("***********************   END   ***********************", Tools.CreateFolderName(source));
                }
                else Tools.LogWriter("***************   Folders were matched   *************", Tools.CreateFolderName(source));
#endif
                #endregion

                if (!CreateFolders(sFoldersAndFiles.GetKeys(), source, destination))
                {
                    #region DEBUG  //Write fail
#if DEBUG
                    Tools.LogWriter("Create folders method fail.", Tools.CreateFolderName(source));
                    Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
                    #endregion
                    return false;
                }

                if (!CopyFiles(ref sFoldersAndFiles, source, destination))
                {
                    #region DEBUG  //Write fail
#if DEBUG
                    Tools.LogWriter("CreateFiles method fail.", Tools.CreateFolderName(source));
                    Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
                    #endregion
                    return false;
                }

                #region DEBUG   //Write list of file that can not be copied
#if DEBUG
                if (sFoldersAndFiles.Key.Count > 0)
                {
                    foreach (var folder in sFoldersAndFiles.Key)
                    {
                        Tools.LogWriter("\t" + folder, Tools.CreateFolderName(source));
                        foreach (var file in sFoldersAndFiles.GetValue(folder))
                        {
                            Tools.LogWriter("\t\t" + file, Tools.CreateFolderName(source));
                        }
                    }
                }
#endif
                #endregion

                FailedFiles ffiles = new FailedFiles(sFoldersAndFiles.Value);
                sFoldersAndFiles.Dispose();
                #region DEBUG  //Write successful
#if DEBUG
                Tools.LogWriter("Synchronization completed successfully.", Tools.CreateFolderName(source));
                Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));

#endif
                #endregion
                pBar.ProgressbarText = "Synchronization completed successfully";
                pBar.ProgressbarValue = 100;
                return true;
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }

            #region DEBUG  //Write fail
#if DEBUG
            Tools.LogWriter("SynchronizeFiles method fail.", Tools.CreateFolderName(source));
            Tools.LogWriter("************************************************************************************************************", Tools.CreateFolderName(source));
#endif
            #endregion
            return false;
        }
    }
}