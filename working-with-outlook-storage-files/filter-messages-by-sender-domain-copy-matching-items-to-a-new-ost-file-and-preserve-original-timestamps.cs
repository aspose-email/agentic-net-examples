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
            // Define source and destination OST file paths and the sender domain to filter.
            string sourceOstPath = "source.ost";
            string destinationOstPath = "filtered.ost";
            string senderDomain = "example.com";

            // Ensure the source file exists; if not, create an empty OST (PST) placeholder.
            if (!File.Exists(sourceOstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(sourceOstPath, FileFormatVersion.Unicode))
                    {
                        // No messages are added; just an empty storage is created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder source OST: {ex.Message}");
                    return;
                }
            }

            // Open the source OST file.
            using (PersonalStorage sourceStorage = PersonalStorage.FromFile(sourceOstPath))
            // Create the destination OST file.
            using (PersonalStorage destinationStorage = PersonalStorage.Create(destinationOstPath, FileFormatVersion.Unicode))
            {
                // Get the root folders of both storages.
                FolderInfo sourceRoot = sourceStorage.RootFolder;
                FolderInfo destinationRoot = destinationStorage.RootFolder;

                // Enumerate all messages in the source root folder.
                foreach (MessageInfo messageInfo in sourceRoot.EnumerateMessages())
                {
                    try
                    {
                        // Extract the full MAPI message.
                        MapiMessage sourceMessage = sourceStorage.ExtractMessage(messageInfo);

                        // Check if the sender's email address ends with the specified domain.
                        string senderEmail = sourceMessage.SenderEmailAddress;
                        if (!string.IsNullOrEmpty(senderEmail) &&
                            senderEmail.EndsWith("@" + senderDomain, StringComparison.OrdinalIgnoreCase))
                        {
                            // Clone the message to preserve all original properties, including timestamps.
                            MapiMessage clonedMessage = sourceMessage.Clone() as MapiMessage;

                            // Explicitly copy timestamp properties (redundant if Clone works, but ensures preservation).
                            clonedMessage.ClientSubmitTime = sourceMessage.ClientSubmitTime;
                            clonedMessage.DeliveryTime = sourceMessage.DeliveryTime;

                            // Add the cloned message to the destination OST root folder.
                            destinationRoot.AddMessage(clonedMessage);
                        }
                    }
                    catch (Exception msgEx)
                    {
                        // Log any message-specific errors and continue processing other messages.
                        Console.Error.WriteLine($"Error processing message ID {messageInfo.EntryIdString}: {msgEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Global exception handling.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
