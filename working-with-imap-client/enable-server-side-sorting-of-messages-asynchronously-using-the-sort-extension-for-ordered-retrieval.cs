using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder server details detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the folder to work with
                    client.SelectFolder("INBOX");

                    // Define sort conditions for the SORT extension
                    SortConditions conditions = new SortConditions
                    {
                        SortBy = SortingKey.Date,
                        UseUId = true
                    };

                    // Perform asynchronous server‑side sorting of message threads
                    List<MessageThreadResult> sortedThreads = await client.SortMessageThreadsAsync(conditions);

                    // Prepare output file path
                    string outputPath = "sorted_threads.txt";
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write sorted thread identifiers to the file
                    try
                    {
                        File.WriteAllLines(outputPath, sortedThreads.Select(r => r.UniqueId.ToString()));
                        Console.WriteLine($"Sorted thread IDs written to {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"File write error: {ioEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
