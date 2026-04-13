using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with real mailbox URI and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("user", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Sample list of item URIs to be archived
                List<string> itemUris = new List<string>
                {
                    "itemUri1",
                    "itemUri2",
                    "itemUri3"
                };

                // Object used for synchronizing access to the shared client
                object clientLock = new object();

                // Perform archiving concurrently while ensuring thread‑safety
                Parallel.ForEach(itemUris, uri =>
                {
                    try
                    {
                        lock (clientLock)
                        {
                            // ArchiveItem(string folderUri, string itemUri)
                            // Passing an empty string for folderUri uses the default behavior
                            client.ArchiveItem(string.Empty, uri);
                        }
                        Console.WriteLine($"Archived: {uri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to archive {uri}: {ex.Message}");
                    }
                });
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
