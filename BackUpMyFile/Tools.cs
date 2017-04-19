using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using TimerHelp;

namespace BackUpMyFile
{
    [Serializable, ProtoContract(Name = @"Archive"), ProtoInclude(1, typeof(Options.OptionData)), ProtoInclude(2, typeof(List<ArchiveForm.ArchiveData>))]
    public class ProgramData : IExtensible
    {
        [ProtoMember(1, IsRequired = false, Name = @"Options", DataFormat = DataFormat.Default)]
        public Options.OptionData Options;

        [ProtoMember(2, IsRequired = false, Name = @"Archive", DataFormat = DataFormat.Default)]
        public List<ArchiveForm.ArchiveData> Archive { set; get; }

        //[ProtoMember(3, IsRequired = false, Name = @"FailedSynchronize", DataFormat = DataFormat.Default)]
        //public List<FoldersAndFiles> FailedSynchronize { set; get; }

        public ProgramData()
        {
            Options = new Options.OptionData();
            Archive = new List<ArchiveForm.ArchiveData>();
            //FailedSynchronize = new List<FoldersAndFiles>();
        }

        private IExtension extensionObject;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
        }
    }

    class Tools
    {
        /// <summary>
        /// Is parameter a folder or not
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <returns>bool</returns>
        public static bool IsADirectoryPath(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                    return true; //MessageBox.Show("It's a directory");
                //else
                    //return false; //MessageBox.Show("It's a file");
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
            return false;
        }

        /// <summary>
        /// Split the path into parts
        /// </summary>
        /// <param name="path">Folder/file path</param>
        /// <returns></returns>
        public static List<string> SplitPath(string path)
        {
            List<string> splitedPath=new List<string>();
            int startIndex = 0;
            for (int i = 0; i < path.Length-1; i++)
            {
                if (path[i]=='\\')
                {
                        splitedPath.Add(path.Substring(startIndex, i-startIndex));
                        startIndex = i+1;
                    
                }
            }
            splitedPath.Add(path.Substring(startIndex, path.Length - startIndex));
            return splitedPath;
        }
        
        /// <summary>
        /// Get name of folder/file by given path
        /// </summary>
        /// <param name="path">Folder/file path</param>
        /// <returns>Name</returns>
        public static string GetNameByPath(string path)
        {
            List<string> splitedPath = SplitPath(path);
            return splitedPath[splitedPath.Count - 1];
        }

        /// <summary>
        /// Validate the Path. If path is relative append the path to the project directory by default.
        /// </summary>
        /// <param name="path">Path to validate</param>
        /// <param name="RelativePath">Relative path</param>
        /// <param name="Extension">If want to check for File Path</param>
        /// <returns></returns>
        public static bool ValidateDllPath(ref string path, string RelativePath = "", string Extension = "")
        {
            // Check if it contains any Invalid Characters.
            if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            {
                try
                {
                    // If path is relative take %IGXLROOT% as the base directory
                    if (!Path.IsPathRooted(path))
                    {
                        if (string.IsNullOrEmpty(RelativePath))
                        {
                            // Exceptions handled by Path.GetFullPath
                            // ArgumentException path is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in GetInvalidPathChars. -or- The system could not retrieve the absolute path.
                            // 
                            // SecurityException The caller does not have the required permissions.
                            // 
                            // ArgumentNullException path is null.
                            // 
                            // NotSupportedException path contains a colon (":") that is not part of a volume identifier (for example, "c:\"). 
                            // PathTooLongException The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.

                            // RelativePath is not passed so we would take the project path 
                            path = Path.GetFullPath(RelativePath);

                        }
                        else
                        {
                            // Make sure the path is relative to the RelativePath and not our project directory
                            path = Path.Combine(RelativePath, path);
                        }
                    }

                    // Exceptions from FileInfo Constructor:
                    //   System.ArgumentNullException:
                    //     fileName is null.
                    //
                    //   System.Security.SecurityException:
                    //     The caller does not have the required permission.
                    //
                    //   System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid characters.
                    //
                    //   System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                    //
                    //   System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                    FileInfo fileInfo = new FileInfo(path);

                    // Exceptions using FileInfo.Length:
                    //   System.IO.IOException:
                    //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
                    //     directory.
                    //
                    //   System.IO.FileNotFoundException:
                    //     The file does not exist.-or- The Length property is called for a directory.
                    bool throwEx = fileInfo.Length == -1;

                    // Exceptions using FileInfo.IsReadOnly:
                    //   System.UnauthorizedAccessException:
                    //     Access to fileName is denied.
                    //     The file described by the current System.IO.FileInfo object is read-only.-or-
                    //     This operation is not supported on the current platform.-or- The caller does
                    //     not have the required permission.
                    throwEx = fileInfo.IsReadOnly;

                    if (!string.IsNullOrEmpty(Extension))
                    {
                        // Validate the Extension of the file.
                        if (Path.GetExtension(path).Equals(Extension, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Trim the Library Path
                            path = path.Trim();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;

                    }
                }
                catch (ArgumentNullException)
                {
                    //   System.ArgumentNullException:
                    //     fileName is null.
                }
                catch (System.Security.SecurityException)
                {
                    //   System.Security.SecurityException:
                    //     The caller does not have the required permission.
                }
                catch (ArgumentException)
                {
                    //   System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid characters.
                }
                catch (UnauthorizedAccessException)
                {
                    //   System.UnauthorizedAccessException:
                    //     Access to fileName is denied.
                }
                catch (PathTooLongException)
                {
                    //   System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                }
                catch (NotSupportedException)
                {
                    //   System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                }
                catch (FileNotFoundException)
                {
                    // System.FileNotFoundException
                    //  The exception that is thrown when an attempt to access a file that does not
                    //  exist on disk fails.
                }
                catch (IOException)
                {
                    //   System.IO.IOException:
                    //     An I/O error occurred while opening the file.
                }
                catch (Exception)
                {
                    // Unknown Exception. Might be due to wrong case or nulll checks.
                }
            }
            else
            {
                // Path contains invalid characters
            }
            return false;
        }

        /// <summary>
        /// Check the presence of the file
        /// </summary>
        /// <param name="fileNames">Name of file</param>
        public static void CheckFile(params string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
            }
        }

        /// <summary>
        /// Check if the file is accessible
        /// </summary>
        /// <param name="sFilename"></param>
        /// <returns></returns>
        public static bool IsFileAccessible(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Get driver letter from path
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns></returns>
        public static string GetDriveLetter(string path)
        {
            return Path.GetPathRoot(path);
        }

        /// <summary>
        /// Get drivers letters from paths
        /// </summary>
        /// <param name="paths">Paths list</param>
        /// <returns></returns>
        public static string[] GetDriveLetters(string[] paths)
        {
            List<string> rootDir = new List<string>();
            foreach (var path in paths)
            {
                rootDir.Add(GetDriveLetter(path));
            }

            return rootDir.ToArray();
        }

        public static string CreateFolderName(string mainPath)
        {
            List<string> splitedpath = SplitPath(mainPath);
            string path = "";
            foreach (var thing in splitedpath)
            {
                if (thing == splitedpath[0])
                    path += thing.Remove(1, 1) + "-";
                else if (thing != splitedpath[splitedpath.Count - 1])
                    path += thing + "-";
                else
                    path += thing;
            }
            path += ".txt";
            return path;
        }

        #region Writers

        public static string logFile = "Log.txt";
        public static string errFile = "Error.txt";

        private static List<string> logtextlist = new List<string>();

        [Conditional("DEBUG")]
        public static void LogWriter(string logText, string filename = null)
        {
            try
            {
                using (StreamWriter writetext = new StreamWriter((filename == null ? logFile : filename), true))
                {
                    if (logtextlist.Count > 0)
                    {
                        foreach (var text in logtextlist)
                            writetext.WriteLine(text);
                        logtextlist.Clear();
                    }
                    writetext.WriteLine(logText);
                    writetext.Close();
                    writetext.Dispose();
                }
            }
            catch (IOException)
            {
                logtextlist.Add(logText);
            }
            catch (Exception ex)
            {
                ErrorWriter(ex);
            }
        }

        [Conditional("DEBUG")]
        public static void ErrorWriter(Exception ex)
        {
            using (StreamWriter writer = new StreamWriter(errFile, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                writer.Close();
                writer.Dispose();
            }
        }
        #endregion

        #region proto buffers
        public const string probuffile= @"BackupData.bin";
        
        /// <summary>
        /// Update file data for OptionData
        /// </summary>
        /// <param name="pd">Archive data that update</param>
        public static void Update(ProgramData pd)
        {
            try
            {
                using (var fs = File.OpenWrite(probuffile))
                {
                    Serializer.Serialize<ProgramData>(fs, pd);
                    fs.SetLength(fs.Position);
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }
        
        /// <summary>
        /// Refresh data from file for Archive
        /// </summary>
        /// <param name="pd">Archive data that refresh</param>
        public static void Refresh(ref ProgramData pd)
        {
            try
            {
                if (File.Exists(probuffile))
                {
                    using (var fs = File.OpenRead(probuffile))
                    {
                        pd = Serializer.Deserialize<ProgramData>(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.ErrorWriter(ex);
            }
        }
        
        /// <summary>
        /// Check the parameter exists in archive data
        /// </summary>
        /// <param name="arc">Archive data that search in archive</param>
        /// <returns></returns>
        public static int Contains(ArchiveForm.ArchiveData arc, bool periode = true)
        {
            for (int i = 0; i < Form1.programData.Archive.Count; i++)
            {
                if (Form1.programData.Archive[i].SourceFolder == arc.SourceFolder &&
                    Form1.programData.Archive[i].DestinationFolder == arc.DestinationFolder &&
                    (periode ? (Form1.programData.Archive[i].BackUpPeriod == arc.BackUpPeriod) : true))
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }
}
