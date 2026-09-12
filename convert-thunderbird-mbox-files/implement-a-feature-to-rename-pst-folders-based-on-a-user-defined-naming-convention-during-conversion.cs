using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST path (will be created if missing)
            const string pstPath = "input.pst";

            // Ensure the PST file exists; create a minimal placeholder if not.
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a sample folder with one message.
                    FolderInfo sampleFolder = placeholder.RootFolder.AddSubFolder("SampleFolder");
                    MailMessage sampleMsg = new MailMessage("sender@example.com", "receiver@example.com", "Sample", "This is a placeholder message.");
                    sampleFolder.AddMessage(MapiMessage.FromMailMessage(sampleMsg));
                }
            }

            // Open the PST file for read/write.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate over each top‑level folder and rename it using a custom convention.
                foreach (FolderInfo originalFolder in pst.RootFolder.GetSubFolders())
                {
                    // Define the new folder name (e.g., prefix with "Renamed_").
                    string newFolderName = $"Renamed_{originalFolder.DisplayName}";

                    // Skip if a folder with the target name already exists.
                    bool alreadyExists = false;
                    foreach (FolderInfo existing in pst.RootFolder.GetSubFolders())
                    {
                        if (string.Equals(existing.DisplayName, newFolderName, StringComparison.OrdinalIgnoreCase))
                        {
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (alreadyExists)
                        continue;

                    // Create the new folder.
                    FolderInfo newFolder = pst.RootFolder.AddSubFolder(newFolderName);

                    // Move all messages from the original folder to the new folder.
                    foreach (MessageInfo msgInfo in originalFolder.EnumerateMessages())
                    {
                        pst.MoveItem(msgInfo, newFolder);
                    }

                    // Note: Sub‑folders are not moved in this simple example.
                }
                // Changes are persisted when the PersonalStorage object is disposed.
            }

            Console.WriteLine("Folder renaming completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
