using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define PST file path
            string pstPath = "MyMail.pst";

            // Ensure the directory for the PST file exists
            string directoryPath = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // If a PST file already exists at the path, delete it to create a fresh one
            if (File.Exists(pstPath))
            {
                File.Delete(pstPath);
            }

            // Create a new Unicode PST file
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Create a predefined "Incoming" folder (mapped to Inbox)
                FolderInfo incomingFolder = pst.CreatePredefinedFolder("Incoming", StandardIpmFolder.Inbox);

                // Create a predefined "Sent Items" folder (mapped to Sent Items)
                FolderInfo sentFolder = pst.CreatePredefinedFolder("Sent Items", StandardIpmFolder.SentItems);

                // Create a custom "Archive" folder (mapped to Notes as a placeholder)
                FolderInfo archiveFolder = pst.CreatePredefinedFolder("Archive", StandardIpmFolder.Notes);

                Console.WriteLine("PST file created with folders: Incoming, Sent Items, Archive.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
