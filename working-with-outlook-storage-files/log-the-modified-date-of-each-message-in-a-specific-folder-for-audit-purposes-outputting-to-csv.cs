using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths – adjust as needed
            string pstPath = "sample.pst";
            string targetFolderName = "Inbox";
            string csvPath = "audit.csv";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Locate the folder (try standard Inbox first, then by name)
                FolderInfo folder;
                try
                {
                    folder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch
                {
                    // Fallback to a subfolder with the given name
                    folder = pst.RootFolder.GetSubFolder(targetFolderName);
                }

                if (folder == null)
                {
                    Console.Error.WriteLine($"Folder not found: {targetFolderName}");
                    return;
                }

                // Create CSV file
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // CSV header
                    writer.WriteLine("Subject,LastModificationTime");

                    // Enumerate messages in the folder
                    foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                    {
                        // Extract the full message to obtain its properties
                        MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                        // Subject (escape quotes for CSV)
                        string subject = (messageInfo.Subject ?? string.Empty).Replace("\"", "\"\"");

                        // Modified time – using the folder's LastModificationTime as a placeholder
                        // (PST does not expose a per‑message modification time directly via MessageInfo)
                        DateTime modifiedTime = folder.LastModificationTime;

                        // Write CSV line
                        writer.WriteLine($"\"{subject}\",{modifiedTime:O}");
                    }
                }
            }

            Console.WriteLine($"Audit CSV created at: {csvPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
