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

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                // Create a new Unicode PST file
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create the Inbox predefined folder
                    FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                    // Add a sample message to the Inbox
                    MapiMessage sampleMsg = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Original Subject",
                        "This is a placeholder message body."
                    );
                    inbox.AddMessage(sampleMsg);
                }

                Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
            }

            // Open the PST file for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder
                FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Enumerate messages in the Inbox
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    // Extract the full message
                    MapiMessage message = pst.ExtractMessage(msgInfo);

                    // Update the Subject property
                    message.Subject = "Updated Subject";

                    // Save the updated message back to the PST
                    // Use the entry ID of the original message
                    inbox.UpdateMessage(msgInfo.EntryIdString, message);

                    Console.WriteLine($"Message with EntryId '{msgInfo.EntryIdString}' updated.");
                    // For demonstration, update only the first message
                    break;
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
