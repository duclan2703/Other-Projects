﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Forms
{
    public enum BrowserEmulationVersion
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Interner Explorer 7 Standards Mode
        /// </summary>
        Version7 = 7000,

        /// <summary>
        /// Interner Explorer 8
        /// </summary>
        Version8 = 8000,

        /// <summary>
        /// Interner Explorer 8 Standards Mode
        /// </summary>
        Version8Standards = 8888,

        /// <summary>
        /// Interner Explorer 9
        /// </summary>
        Version9 = 9000,

        /// <summary>
        /// Interner Explorer 9 Standards Mode
        /// </summary>
        Version9Standards = 9999,

        /// <summary>
        /// Interner Explorer 10
        /// </summary>
        Version10 = 10000,

        /// <summary>
        /// Interner Explorer 10 Standards Mode
        /// </summary>
        Version10Standards = 10001,

        /// <summary>
        /// Interner Explorer 11
        /// </summary>
        Version11 = 11000,

        /// <summary>
        /// Interner Explorer 11 Edge Mode
        /// </summary>
        Version11Edge = 11001
    }

    /// <summary>
    /// Helper methods for working with Internet Explorer browser emulation
    /// </summary>
    public static class InternetExplorerBrowserEmulation
    {
        #region Constants

        private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";

        private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

        #endregion

        #region Public Class Members

        /// <summary>
        /// Gets the browser emulation version for the application.
        /// </summary>
        /// <returns>The browser emulation version for the application.</returns>
        public static BrowserEmulationVersion GetBrowserEmulationVersion()
        {
            BrowserEmulationVersion result;

            result = BrowserEmulationVersion.Default;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                if (key != null)
                {
                    string programName;
                    object value;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                    value = key.GetValue(programName, null);

                    if (value != null)
                    {
                        result = (BrowserEmulationVersion)Convert.ToInt32(value);
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }

        /// <summary>
        /// Gets the major Internet Explorer version
        /// </summary>
        /// <returns>The major digit of the Internet Explorer version</returns>
        public static int GetInternetExplorerMajorVersion()
        {
            int result;

            result = 0;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                if (key != null)
                {
                    object value;

                    value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                    if (value != null)
                    {
                        string version;
                        int separator;

                        version = value.ToString();
                        separator = version.IndexOf('.');
                        if (separator != -1)
                        {
                            int.TryParse(version.Substring(0, separator), out result);
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }

        /// <summary>
        /// Determines whether a browser emulation version is set for the application.
        /// </summary>
        /// <returns><c>true</c> if a specific browser emulation version has been set for the application; otherwise, <c>false</c>.</returns>
        public static bool IsBrowserEmulationSet()
        {
            return GetBrowserEmulationVersion() != BrowserEmulationVersion.Default;
        }

        /// <summary>
        /// Sets the browser emulation version for the application.
        /// </summary>
        /// <param name="browserEmulationVersion">The browser emulation version.</param>
        /// <returns><c>true</c> the browser emulation version was updated, <c>false</c> otherwise.</returns>
        public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
        {
            bool result;

            result = false;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

                if (key == null)

                    key = Registry.CurrentUser.CreateSubKey(BrowserEmulationKey);
                string programName;

                programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

                if (browserEmulationVersion != BrowserEmulationVersion.Default)
                {
                    // if it's a valid value, update or create the value
                    key.SetValue(programName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                }
                else
                {
                    // otherwise, remove the existing value
                    key.DeleteValue(programName, false);
                }

                result = true;

            }
            catch (SecurityException)
            {
                // The user does not have the permissions required to read from the registry key.
            }
            catch (UnauthorizedAccessException)
            {
                // The user does not have the necessary registry rights.
            }

            return result;
        }

        /// <summary>
        /// Sets the browser emulation version for the application to the highest default mode for the version of Internet Explorer installed on the system
        /// </summary>
        /// <returns><c>true</c> the browser emulation version was updated, <c>false</c> otherwise.</returns>
        public static bool SetBrowserEmulationVersion()
        {
            int ieVersion;
            BrowserEmulationVersion emulationCode;

            ieVersion = GetInternetExplorerMajorVersion();

            if (ieVersion >= 11)
            {
                emulationCode = BrowserEmulationVersion.Version11;
            }
            else
            {
                switch (ieVersion)
                {
                    case 10:
                        emulationCode = BrowserEmulationVersion.Version10;
                        break;
                    case 9:
                        emulationCode = BrowserEmulationVersion.Version9;
                        break;
                    case 8:
                        emulationCode = BrowserEmulationVersion.Version8;
                        break;
                    default:
                        emulationCode = BrowserEmulationVersion.Version7;
                        break;
                }
            }

            return SetBrowserEmulationVersion(emulationCode);
        }

        #endregion
    }
}
