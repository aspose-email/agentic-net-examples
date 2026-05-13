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
            string pstPath = "sample.pst";
            // Keyword to search in subject
            string keyword = "Important";
            // Tag to prepend
            string tag = "[Tagged]";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for reading and writing
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    if (!pst.CanWrite)
                    {
                        Console.Error.WriteLine("PST file is opened in read‑only mode.");
                        return;
                    }

                    // Process the root folder recursively
                    ProcessFolder(pst, pst.RootFolder, keyword, tag);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }

            Console.WriteLine("Subject line update completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively processes a folder and its subfolders
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string keyword, string tag)
    {
        // Enumerate all messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Check if the subject contains the keyword (case‑insensitive)
            if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Prepend the tag to the subject
                        mapiMessage.Subject = $"{tag} {mapiMessage.Subject}";

                        // Update the message in the folder
                        folder.UpdateMessage(messageInfo.EntryIdString, mapiMessage);
                        Console.WriteLine($"Updated message ID {messageInfo.EntryIdString}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update message ID {messageInfo.EntryIdString}: {ex.Message}");
                }
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, keyword, tag);
        }
    }
}
