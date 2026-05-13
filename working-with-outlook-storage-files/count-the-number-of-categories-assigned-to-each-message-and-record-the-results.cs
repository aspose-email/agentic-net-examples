using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST and process its contents.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst, pst.RootFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        // Enumerate all messages in the current folder.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract the full MAPI message.
                MapiMessage message = pst.ExtractMessage(messageInfo);

                // Count the categories assigned to this message.
                string[] categories = message.Categories ?? Array.Empty<string>();
                int categoryCount = categories.Length;

                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Category count: {categoryCount}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {ex.Message}");
            }
        }

        // Recursively process subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder);
        }
    }
}
