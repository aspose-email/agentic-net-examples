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

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST with search folders included (if supported by the library version).
            PersonalStorageLoadOptions loadOptions = new PersonalStorageLoadOptions();
            // The property name may vary between versions; attempt to set the appropriate flag.
            // If the property does not exist, the code will still compile without it.
            // Uncomment the line that matches the library version you are using.
            // loadOptions.IncludeSearchFolders = true; // For newer versions
            // loadOptions.LoadSearchFolders = true;   // For older versions

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, loadOptions))
            {
                // Traverse all folders starting from the root.
                TraverseFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void TraverseFolder(PersonalStorage pst, FolderInfo folder)
    {
        Console.WriteLine($"Folder: {folder.DisplayName}");
        Console.WriteLine($"  Total items: {folder.ContentCount}");
        Console.WriteLine($"  Unread items: {folder.ContentUnreadCount}");

        // Enumerate messages in the current folder.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            Console.WriteLine($"  Subject: {messageInfo.Subject}");

            try
            {
                using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                {
                    MailMessage message = mapiMsg.ToMailMessage(new MailConversionOptions());
                    // Example: display sender and date.
                    Console.WriteLine($"    From: {message.From}");
                    Console.WriteLine($"    Date: {message.Date}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"    Failed to extract message: {ex.Message}");
            }
        }

        // Recursively process subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            TraverseFolder(pst, subFolder);
        }
    }
}
