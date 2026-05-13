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
            string inputPstPath = "input.pst";
            string outputPstPath = "output.pst";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a minimal placeholder PST if the input file does not exist
            if (!File.Exists(inputPstPath))
            {
                using (PersonalStorage placeholderPst = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                {
                    FolderInfo inbox = placeholderPst.RootFolder.AddSubFolder("Inbox");
                    MapiMessage placeholderMsg = new MapiMessage("from@example.com", "to@example.com", "Placeholder", "This is a placeholder message.");
                    inbox.AddMessage(placeholderMsg);
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Process each folder recursively
                ProcessFolder(pst.RootFolder, pst);

                // Save the modified PST to a different file to avoid overwriting the opened file
                pst.SaveAs(outputPstPath, FileFormat.Pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
    {
        // Iterate through messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract the full MAPI message
            MapiMessage message = pst.ExtractMessage(messageInfo);

            // Ensure the Categories collection is initialized
            if (message.Categories == null)
            {
                message.Categories = new string[0];
            }

            // Add default category if none exist
            if (message.Categories.Length == 0)
            {
                FollowUpManager.AddCategory(message, "Default");
            }

            // Update the message back into the PST
            folder.UpdateMessage(messageInfo.EntryIdString, message);
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst);
        }
    }
}
