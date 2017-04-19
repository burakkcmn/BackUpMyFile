using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using static System.Environment;

namespace BackUpMyFile
{
    static class Startup
    {
        /// <summary>
        /// Add application to windows startup registry.
        /// </summary>
        /// <param name="appPath">Path of application</param>
        /// <param name="appName">Name of application</param>
        /// <returns></returns>
        public static bool AddToReg(string appPath, string appName = null)
        {
            try
            {
                using (RegistryKey startupPath = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (appName == null)
                    {
                        FileInfo fi = new FileInfo(appPath);
                        appName = fi.Name;
                    }
                    startupPath.SetValue(appName, Application.ExecutablePath);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Remove application from windows startup registry.
        /// </summary>
        /// <param name="appName">Name of application</param>
        /// <returns></returns>
        public static bool RemoveFromReg(string appName)
        {
            try
            {
                using (RegistryKey startupPath = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    startupPath.DeleteValue("MyApp", false);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add application to windows startup folder.
        /// </summary>
        /// <param name="appPath">Path of application</param>
        /// <param name="appName">Name of application</param>
        /// <returns></returns>
        public static bool AddToStartupFolder(string appPath, string appName = null)
        {
            try
            {
                if (appName == null)
                {
                    FileInfo fi = new FileInfo(appPath);
                    appName = fi.Name;
                }
                //System.IO.File.Copy(appPath, Path.Combine(Environment.GetFolderPath(SpecialFolder.CommonStartup), appName));

                WshShell shell = new WshShell();
                string shortcutAddress = appPath;
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "New shortcut for " + appName;
                shortcut.Hotkey = "Ctrl+Shift+N";
                shortcut.TargetPath = Path.Combine(Environment.GetFolderPath(SpecialFolder.CommonStartup), appName);
                shortcut.Save();
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Remove application from windows startup folder.
        /// </summary>
        /// <param name="appName">Name of application</param>
        /// <returns></returns>
        public static bool RemoveFromStartupFolder(string appName)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(Environment.GetFolderPath(SpecialFolder.CommonStartup), appName));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
