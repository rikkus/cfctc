using System;
using System.IO;
using System.Windows.Forms;
using CopyFileContentToClipboard.Properties;

[assembly: CLSCompliant(true)]

namespace CopyFileContentToClipboard
{
    internal static class Program
    {
        private const string FileType = "*";
        private const string KeyName = "Copy text content to clipboard context menu";
        private const string MenuText = "Copy text content to clipboard";
        private const string SizeWarning = "Are you sure you want to copy {0} of data to the clipboard?";
        private const string SizeWarningTitle = "Warning";
        private const int WarnSize = 1024 * 1024 * 10; // Is 10MB a good number?

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                WrappedMain(args);
            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                    String.Format("Unable to copy file text: {0}", ex.Message),
                    Resources.Program_Main_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private static void WrappedMain(string[] args)
        {
            if (args.Length == 0 || string.Compare(args[0], "-register", true) == 0)
            {
                try
                {
                    ShellExtension.Register
                        (FileType, KeyName, MenuText, string.Format("\"{0}\" \"%L\"", Application.ExecutablePath));

                    MessageBox.Show(string.Format("Registered extension handling '{0}'.", FileType), Resources.Program_WrappedMain_Registered_OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show
                        (
                        string.Format("Unable to register shell extension: {0}", ex.Message),
                        Resources.Program_Main_Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
            }
            else if (string.Compare(args[0], "-unregister", true) == 0)
            {
                ShellExtension.Unregister(FileType, KeyName);
            }
            else
            {
                CopyFileContentToClipboard(args[0]);
            }
        }

        private static void CopyFileContentToClipboard(string filePath)
        {
            long fileSize = FileSize(filePath);

            if (fileSize < WarnSize || UserAcceptsFileSize(fileSize))
            {
                string text = TextContent(filePath);

                if (String.IsNullOrEmpty(text))
                {
                    Clipboard.Clear();
                }
                else
                {
                    Clipboard.SetText(text);
                }
            }
        }

        private static bool UserAcceptsFileSize(long dataLength)
        {
            return MessageBox.Show
                       (
                       String.Format(SizeWarning, Utils.PrettyPrint(dataLength)),
                       SizeWarningTitle,
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question,
                       MessageBoxDefaultButton.Button2
                       ) == DialogResult.Yes;
        }

        private static long FileSize(string filePath)
        {
            try
            {
                return (new FileInfo(filePath)).Length;
            }
            catch (Exception ex)
            {
                throw new FileNotReadableException(filePath, ex);
            }
        }

        private static string TextContent(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new FileNotReadableException(filePath, ex);
            }
        }
    }
}