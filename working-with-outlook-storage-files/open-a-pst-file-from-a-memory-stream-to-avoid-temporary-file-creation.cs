using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        const string pstFilePath = "sample.pst";

        // Ensure a placeholder PST file exists
        if (!File.Exists(pstFilePath))
        {
            try
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create a simple folder structure
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");
                    // Optionally add a dummy message
                    // MessageInfo dummy = inbox.AddMessage(Message.Create("sender@example.com", "receiver@example.com", "Test", "Body"));
                }
                Console.WriteLine($"Placeholder PST created at '{pstFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                return;
            }
        }

        try
        {
            // Open the PST file from a memory stream to avoid creating a temporary file on disk
            using (FileStream fileStream = new FileStream(pstFilePath, FileMode.Open, FileAccess.Read))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                memoryStream.Position = 0; // Reset position for reading

                using (PersonalStorage pst = PersonalStorage.FromStream(memoryStream))
                {
                    // List folders and message subjects
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            Console.WriteLine($"  Subject: {messageInfo.Subject}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing PST: {ex.Message}");
        }
    }
}
