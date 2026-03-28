using System;
using System.Collections.Generic;
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
            // Path to the PST file
            string pstPath = "BulkMessages.pst";

            // Ensure the PST file exists; create a new Unicode PST if it does not
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open the PST file for read/write operations
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve the Inbox folder (creates it if missing)
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Prepare a collection of MAPI messages to add in bulk
                List<MapiMessage> bulkMessages = new List<MapiMessage>();
                for (int i = 1; i <= 1000; i++)
                {
                    MapiMessage message = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        $"Test Subject {i}",
                        $"This is the body of message {i}."
                    );
                    bulkMessages.Add(message);
                }

                // Add all messages to the folder using the bulk API
                inboxFolder.AddMessages(bulkMessages);
                Console.WriteLine($"{bulkMessages.Count} messages have been added to the PST.");

                // Dispose each message after it has been added
                foreach (MapiMessage message in bulkMessages)
                {
                    message.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
