using System;
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
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a predefined Inbox folder
                        FolderInfo inbox = pstCreate.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Create a simple message
                        MapiMessage simpleMsg = new MapiMessage(
                            "sender@example.com",
                            "recipient@example.com",
                            "Test Subject",
                            "This is a test message.");

                        // Add the message to the Inbox and obtain its EntryId
                        string validEntryId = inbox.AddMessage(simpleMsg);
                        Console.WriteLine($"Created sample PST with message EntryId: {validEntryId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for reading
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder (or first subfolder if Inbox not present)
                FolderInfo folder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (folder == null)
                {
                    Console.Error.WriteLine("Inbox folder not found in PST.");
                    return;
                }

                // Enumerate messages in the folder
                foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                {
                    try
                    {
                        // Attempt to extract the message using the MessageInfo object
                        using (MapiMessage extractedMsg = pst.ExtractMessage(messageInfo))
                        {
                            Console.WriteLine($"Extracted message: Subject = {extractedMsg.Subject}");
                        }
                    }
                    catch (Aspose.Email.AsposeException ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message with Subject '{messageInfo.Subject}': {ex.Message}");
                    }
                }

                // Demonstrate handling of an invalid EntryId
                const string invalidEntryId = "InvalidEntryIdString";
                try
                {
                    using (MapiMessage invalidMsg = pst.ExtractMessage(invalidEntryId))
                    {
                        // This line should not be reached if the EntryId is invalid
                        Console.WriteLine($"Unexpectedly extracted message with Subject = {invalidMsg.Subject}");
                    }
                }
                catch (Aspose.Email.AsposeException ex)
                {
                    Console.Error.WriteLine($"Handled invalid EntryId gracefully: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error while handling invalid EntryId: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
