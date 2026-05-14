using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Add an Inbox folder
                        FolderInfo inbox = createdPst.RootFolder.AddSubFolder("Inbox");
                        // Create a simple message
                        MapiMessage sampleMsg = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body.");
                        // Add the message to Inbox
                        inbox.AddMessage(sampleMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process each folder recursively
                ProcessFolder(pst.RootFolder, pst);
                // Save the modified PST to a new file (archive)
                string archivePath = "archived.pst";
                try
                {
                    // Overwrite if exists
                    if (File.Exists(archivePath))
                    {
                        File.Delete(archivePath);
                    }
                    pst.SaveAs(archivePath, FileFormat.Pst);
                    Console.WriteLine($"PST archived successfully to '{archivePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save archived PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively process a folder and its subfolders
    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
    {
        // Update each message in the current folder
        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract the full message
                using (MapiMessage message = pst.ExtractMessage(msgInfo))
                {
                    // Append disclaimer to the body
                    string disclaimer = "\n\n---\nCustom Disclaimer: This email is confidential.";
                    message.Body = (message.Body ?? string.Empty) + disclaimer;

                    // Update the message in the PST
                    folder.UpdateMessage(msgInfo.EntryIdString, message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to update message '{msgInfo.Subject}': {ex.Message}");
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst);
        }
    }
}
