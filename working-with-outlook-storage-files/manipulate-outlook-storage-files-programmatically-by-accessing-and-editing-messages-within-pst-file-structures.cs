using System;
using System.IO;
using System.Linq;
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

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the root folder (or create Inbox if it doesn't exist)
                FolderInfo inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                if (inboxFolder == null)
                {
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Add a new message to the folder
                MapiMessage newMessage = new MapiMessage(
                    "alice@example.com",
                    "bob@example.com",
                    "Hello from Aspose.Email",
                    "This is a test message added to the PST."
                );

                string newEntryId = inboxFolder.AddMessage(newMessage);
                Console.WriteLine($"Added new message with EntryId: {newEntryId}");

                // Enumerate messages in the folder
                MessageInfo[] messages = inboxFolder.EnumerateMessages().ToArray();
                if (messages.Length == 0)
                {
                    Console.WriteLine("No messages found in the folder.");
                    return;
                }

                // Extract the first message
                MessageInfo firstInfo = messages[0];
                MapiMessage extractedMessage;
                try
                {
                    extractedMessage = pst.ExtractMessage(firstInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error extracting message: {ex.Message}");
                    return;
                }

                // Display original subject
                Console.WriteLine($"Original Subject: {extractedMessage.Subject}");

                // Modify the subject of the extracted message
                extractedMessage.Subject = "Updated Subject - " + DateTime.Now.ToString("yyyyMMddHHmmss");

                // Add the modified message as a new item
                string updatedEntryId = inboxFolder.AddMessage(extractedMessage);
                Console.WriteLine($"Added updated message with EntryId: {updatedEntryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
