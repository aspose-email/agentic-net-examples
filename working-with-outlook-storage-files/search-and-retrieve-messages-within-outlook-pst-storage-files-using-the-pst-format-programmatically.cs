using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional content needed for the placeholder.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Search keyword.
                    string keyword = "Test";

                    // Process the root folder.
                    ProcessFolder(pst.RootFolder, keyword, pst);

                    // Recursively process subfolders.
                    foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                    {
                        ProcessFolder(subFolder, keyword, pst);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, string keyword, PersonalStorage pst)
    {
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                if (messageInfo.Subject != null && messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Extract the full message.
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        Console.WriteLine($"Found message in folder '{folder.DisplayName}': {messageInfo.Subject}");
                        // Example: display sender and body preview.
                        Console.WriteLine($"  From: {message.SenderEmailAddress}");
                        Console.WriteLine($"  Body preview: {message.Body?.Substring(0, Math.Min(100, message.Body?.Length ?? 0))}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder '{folder.DisplayName}': {ex.Message}");
        }
    }
}
