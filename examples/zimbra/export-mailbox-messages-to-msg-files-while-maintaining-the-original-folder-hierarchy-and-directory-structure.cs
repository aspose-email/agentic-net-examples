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
            // Input PST file path
            string pstFilePath = "input.pst";
            // Output directory for exported MSG files
            string outputRoot = "ExportedMessages";

            // Guard input PST file existence
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstFilePath}");
                return;
            }

            // Ensure output root directory exists
            try
            {
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Process the root folder and its subfolders recursively
                ProcessFolder(pst.RootFolder, outputRoot, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    // Recursively processes a folder, creates corresponding directory, and saves messages as MSG files
    private static void ProcessFolder(FolderInfo folder, string outputRoot, PersonalStorage pst)
    {
        // Retrieve the full path of the folder within the PST (e.g., "Inbox/SubFolder")
        string folderPath = folder.RetrieveFullPath();

        // Combine with the output root to get the target directory path
        string targetDir = Path.Combine(outputRoot, folderPath);

        // Ensure the target directory exists
        try
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating directory '{targetDir}': {ex.Message}");
            return;
        }

        // Export each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract the message as a MapiMessage
            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
            {
                // Build a safe file name using the message subject (fallback to EntryId if needed)
                string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "Message" : msg.Subject;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeSubject = safeSubject.Replace(c, '_');
                }

                string msgFileName = $"{safeSubject}_{messageInfo.EntryIdString}.msg";
                string msgFilePath = Path.Combine(targetDir, msgFileName);

                try
                {
                    // Save the message as an MSG file
                    msg.Save(msgFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving message to '{msgFilePath}': {ex.Message}");
                }
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, outputRoot, pst);
        }
    }
}