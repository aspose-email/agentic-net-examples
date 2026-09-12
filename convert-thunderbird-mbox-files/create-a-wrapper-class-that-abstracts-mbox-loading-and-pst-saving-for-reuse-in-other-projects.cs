using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace MboxPstConversionDemo
{
    // Author: Sample code demonstrating a reusable wrapper for MBOX to PST conversion.
    public static class MboxPstConverter
    {
        // Converts an MBOX file to a PST file using Aspose.Email's MailStorageConverter.
        // Returns the created PersonalStorage instance, or null if conversion fails.
        public static PersonalStorage Convert(string mboxFilePath, string pstFilePath)
        {
            // Guard file system access.
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return null;
            }

            try
            {
                // Ensure the directory for the PST file exists.
                string pstDirectory = Path.GetDirectoryName(pstFilePath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Perform the conversion. The method returns a PersonalStorage object.
                PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);
                return pstStorage;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return null;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // Example input and output paths.
                string mboxPath = "sample.mbox";
                string pstPath = "output.pst";

                // Use the wrapper to convert.
                PersonalStorage pst = MboxPstConverter.Convert(mboxPath, pstPath);
                if (pst != null)
                {
                    // Dispose the PST storage when done.
                    pst.Dispose();
                    Console.WriteLine("MBOX successfully converted to PST.");
                }
                else
                {
                    Console.Error.WriteLine("Conversion did not produce a PST file.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
