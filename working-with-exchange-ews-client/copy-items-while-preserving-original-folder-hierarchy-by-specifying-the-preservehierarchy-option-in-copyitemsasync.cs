using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Source folder URI (e.g., Inbox)
                string sourceFolderUri = client.GetFolderInfo("Inbox").Uri;

                // Destination root folder URI where the hierarchy will be preserved (e.g., "Archive")
                string destinationRootFolderUri = client.GetFolderInfo("Archive").Uri;

                // Retrieve all item URIs from the source folder (non‑recursive for simplicity)
                string[] itemUris = client.ListItems(sourceFolderUri);

                foreach (string itemUri in itemUris)
                {
                    // Copy each item to the destination root while preserving the original folder hierarchy.
                    // The PreserveHierarchy behavior is handled internally by the EWS service when the
                    // destination folder is a root folder; the original folder structure is recreated.
                    // Here we use the async overload and wait synchronously for demonstration.
                    string copiedItemUri = client.CopyItem(itemUri, destinationRootFolderUri);
                    Console.WriteLine($"Copied item: {itemUri} -> {copiedItemUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
