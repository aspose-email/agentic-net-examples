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
            // Author note: This sample demonstrates bulk insertion of messages into a PST file.
            string pstPath = "BulkMessages.pst";

            // Ensure the PST file exists; create a new one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new PST with Unicode format.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Use the root folder for bulk insertion.
                FolderInfo rootFolder = pst.RootFolder;

                // Prepare a collection of MapiMessage objects.
                List<MapiMessage> messages = new List<MapiMessage>();

                // Example: create 5 dummy messages.
                for (int i = 1; i <= 5; i++)
                {
                    MapiMessage msg = new MapiMessage
                    {
                        Subject = $"Bulk Message {i}",
                        Body = $"This is the body of bulk message {i}.",
                        SenderName = "Bulk Sender",
                        SenderEmailAddress = "sender@example.com"
                    };
                    // Add a recipient.
                    msg.Recipients.Add("recipient@example.com", "Recipient", MapiRecipientType.MAPI_TO);
                    messages.Add(msg);
                }

                try
                {
                    // Add all messages to the folder in bulk.
                    rootFolder.AddMessages(messages);
                    Console.WriteLine($"Successfully added {messages.Count} messages to the PST.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
