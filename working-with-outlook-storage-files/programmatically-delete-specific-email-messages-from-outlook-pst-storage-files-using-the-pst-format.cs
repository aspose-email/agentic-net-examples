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

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder of the root folder.
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    // Enumerate messages in the current folder.
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        // Example condition: delete messages whose subject contains "DeleteMe".
                        if (!string.IsNullOrEmpty(msgInfo.Subject) && msgInfo.Subject.Contains("DeleteMe"))
                        {
                            try
                            {
                                // Delete the message by its string entry ID.
                                pst.DeleteItem(msgInfo.EntryIdString);
                                Console.WriteLine($"Deleted message with Subject: '{msgInfo.Subject}'");
                            }
                            catch (Exception delEx)
                            {
                                Console.Error.WriteLine($"Failed to delete message '{msgInfo.Subject}': {delEx.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}
