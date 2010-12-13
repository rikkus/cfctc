using System;
using Microsoft.Win32;

namespace CopyFileContentToClipboard
{
    public class ShellExtension
    {
        public static void Register
            (
            string fileType,
            string shellKeyName,
            string menuText,
            string menuCommand
            )
        {
            string regPath = string.Format(@"{0}\shell\{1}", fileType, shellKeyName);

            using (var key = Registry.ClassesRoot.CreateSubKey(regPath))
            {
                if (key == null)
                    throw new NullReferenceException(string.Format(@"Unable to open registry key {0}", regPath));

                key.SetValue(null, menuText);
            }

            using (var key = Registry.ClassesRoot.CreateSubKey(string.Format(@"{0}\command", regPath)))
            {
                if (key == null)
                    throw new NullReferenceException(string.Format(@"Unable to open registry key {0}\command", regPath));

                key.SetValue(null, menuCommand);
            }
        }

        public static void Unregister(string fileType, string shellKeyName)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(string.Format(@"{0}\shell\{1}", fileType, shellKeyName));
        }
    }
}
