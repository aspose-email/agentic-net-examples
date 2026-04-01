using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

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
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default Inbox folder
                        placeholder.RootFolder.AddSubFolder("Inbox");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Define the case‑insensitive search term
                string searchTerm = "invoice";

                // Iterate through all subfolders recursively
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    SearchFolder(folder, searchTerm);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively search a folder for messages whose Subject contains the term (case‑insensitive)
    private static void SearchFolder(FolderInfo folder, string term)
    {
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                    messageInfo.Subject.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine($"Found in folder \"{folder.DisplayName}\": Subject = {messageInfo.Subject}");
                }
            }

            // Recurse into subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                SearchFolder(subFolder, term);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder \"{folder.DisplayName}\": {ex.Message}");
        }
    }
}
