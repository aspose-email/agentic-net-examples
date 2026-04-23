using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace MboxPstUtility
{
    /// <summary>
    /// Provides methods to convert an MBOX file to a PST file.
    /// </summary>
    public static class MboxPstConverter
    {
        /// <summary>
        /// Converts the specified MBOX file to a PST file.
        /// </summary>
        /// <param name="mboxFilePath">Full path to the source MBOX file.</param>
        /// <param name="pstFilePath">Full path where the PST file will be created.</param>
        public static void Convert(string mboxFilePath, string pstFilePath)
        {
            // Guard against missing input file
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                // Perform the conversion; the method returns a PersonalStorage instance that must be disposed.
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                {
                    // The conversion is complete at this point.
                    Console.WriteLine($"Successfully converted '{mboxFilePath}' to '{pstFilePath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Example usage: replace with actual paths as needed.
                string mboxPath = "sample.mbox";
                string pstPath = "output.pst";

                MboxPstConverter.Convert(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
