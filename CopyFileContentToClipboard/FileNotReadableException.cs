using System;

namespace CopyFileContentToClipboard
{
    internal class FileNotReadableException : Exception
    {
        public FileNotReadableException(string filePath, Exception innerException)
            : base(String.Format("Unable to read file '{0}': {1}", filePath, innerException.Message))
        {
        }
    }
}