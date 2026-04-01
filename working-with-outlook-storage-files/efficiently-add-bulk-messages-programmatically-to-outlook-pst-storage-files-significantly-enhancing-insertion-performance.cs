using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for PST file and optional messages folder
            string pstFilePath = "BulkMessages.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {pstDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Create a collection of MAPI messages to be added in bulk
            List<MapiMessage> bulkMessages = new List<MapiMessage>();
            for (int i = 1; i <= 1000; i++)
            {
                MapiMessage message = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    $"Bulk Subject {i}",
                    $"This is the body of bulk message #{i}.");
                bulkMessages.Add(message);
            }

            // Open existing PST or create a new one
            PersonalStorage pstStorage = null;
            try
            {
                if (File.Exists(pstFilePath))
                {
                    pstStorage = PersonalStorage.FromFile(pstFilePath);
                }
                else
                {
                    pstStorage = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error: Unable to open or create PST file – {pstFilePath}. {pstEx.Message}");
                return;
            }

            using (pstStorage)
            {
                // Get the Inbox folder (creates it if it does not exist)
                FolderInfo inboxFolder = pstStorage.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (inboxFolder == null)
                {
                    Console.Error.WriteLine("Error: Unable to retrieve the Inbox folder.");
                    return;
                }

                // Add messages in bulk – this is much faster than adding one by one
                try
                {
                    inboxFolder.AddMessages(bulkMessages);
                    Console.WriteLine($"Successfully added {bulkMessages.Count} messages to the PST.");
                }
                catch (Exception addEx)
                {
                    Console.Error.WriteLine($"Error: Failed to add messages in bulk. {addEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
