using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Verify that the PST file exists before attempting to open it.
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open the PST file with write access.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Retrieve the Inbox folder (standard IPM folder).
                FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (inbox == null)
                {
                    Console.Error.WriteLine("Error: Inbox folder not found in the PST.");
                    return;
                }

                // Collect entry IDs of all messages in the folder.
                List<string> entryIds = new List<string>();
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    // EntryIdString provides the string representation of the entry ID.
                    entryIds.Add(msgInfo.EntryIdString);
                }

                // Perform bulk deletion of the collected messages.
                if (entryIds.Count > 0)
                {
                    inbox.DeleteChildItems(entryIds);
                    Console.WriteLine($"Deleted {entryIds.Count} messages from the Inbox.");
                }
                else
                {
                    Console.WriteLine("No messages found to delete.");
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors.
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
