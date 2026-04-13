using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Example list of item IDs to archive
                List<string> itemIds = new List<string>
                {
                    "item-id-1",
                    "item-id-2",
                    "item-id-3"
                };

                // Destination archive folder URI (replace with actual archive folder URI)
                string archiveFolderUri = "archive-folder-uri";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                foreach (string itemId in itemIds)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    try
                    {
                        client.ArchiveItem(archiveFolderUri, itemId);
                        sw.Stop();
                        Console.WriteLine($"Archived item '{itemId}' in {sw.ElapsedMilliseconds} ms.");
                    }
                    catch (Exception ex)
                    {
                        sw.Stop();
                        Console.Error.WriteLine($"Failed to archive item '{itemId}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
