using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with actual values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Folder URIs (replace with actual values)
            string sourceFolderUri = "https://exchange.example.com/EWS/Inbox";
            string destinationFolderUri = "https://exchange.example.com/EWS/Archive";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password" ||
                sourceFolderUri.Contains("example.com") ||
                destinationFolderUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Sample item URIs to process (replace with actual item URIs)
            List<string> itemUris = new List<string>
            {
                "https://exchange.example.com/EWS/Message/1",
                "https://exchange.example.com/EWS/Message/2",
                "https://exchange.example.com/EWS/Message/3"
            };

            int archivedCount = 0;
            int movedCount = 0;
            int failedCount = 0;

            // Create and use the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                foreach (string itemUri in itemUris)
                {
                    try
                    {
                        // Archive the item
                        client.ArchiveItem(sourceFolderUri, itemUri);
                        archivedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to archive item '{itemUri}': {ex.Message}");
                        failedCount++;
                        continue; // Skip moving if archiving failed
                    }

                    try
                    {
                        // Move the item to the destination folder
                        client.MoveItem(destinationFolderUri, itemUri);
                        movedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move item '{itemUri}': {ex.Message}");
                        failedCount++;
                    }
                }
            }

            // Output summary report
            Console.WriteLine("Processing Summary:");
            Console.WriteLine($"Archived items: {archivedCount}");
            Console.WriteLine($"Moved items: {movedCount}");
            Console.WriteLine($"Failed items: {failedCount}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
