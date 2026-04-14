using Aspose.Email.Storage.Pst;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with actual server details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client with safety wrapper
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get information about the source folder (Inbox)
                ExchangeFolderInfo inboxInfo = client.GetFolderInfo("Inbox");
                if (inboxInfo == null)
                {
                    Console.Error.WriteLine("Inbox folder not found.");
                    return;
                }

                // Retrieve messages from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxInfo.Uri);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages to archive.");
                    return;
                }

                // Archive each message in parallel
                ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                Parallel.ForEach(messages, parallelOptions, messageInfo =>
                {
                    try
                    {
                        // Archive the message using its unique URI
                        client.ArchiveItem(inboxInfo.Uri, messageInfo.UniqueUri);
                    }
                    catch (Exception ex)
                    {
                        // Log any errors for individual messages without stopping the whole process
                        Console.Error.WriteLine($"Failed to archive message {messageInfo.UniqueUri}: {ex.Message}");
                    }
                });

                Console.WriteLine("Archiving completed.");
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
