using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstSearch
{
    // Author: Aspose.Email example – case‑insensitive search in PST storage
    class Program
    {
        static void Main()
        {
            const string pstPath = "storage.pst";
            const string outputDir = "output";
            const string searchTerm = "invoice"; // case‑insensitive term to search for

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                return;
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder
                    ProcessFolder(pst, pst.RootFolder, searchTerm, outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
            }
        }

        private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string searchTerm, string outputDir)
        {
            // Enumerate messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                    {
                        if (!string.IsNullOrEmpty(msg.Subject) &&
                            msg.Subject.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string safeFileName = GetSafeFileName(msg.Subject);
                            string outputPath = Path.Combine(outputDir, safeFileName + ".msg");
                            msg.Save(outputPath);
                            Console.WriteLine($"Saved matching message: {outputPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to extract/save message '{messageInfo.Subject}': {ex.Message}");
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder, searchTerm, outputDir);
            }
        }

        private static string GetSafeFileName(string original)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                original = original.Replace(c, '_');
            }
            // Trim length if necessary
            return original.Length > 100 ? original.Substring(0, 100) : original;
        }
    }
}
