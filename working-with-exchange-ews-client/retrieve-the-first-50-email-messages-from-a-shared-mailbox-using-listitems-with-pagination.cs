using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URL and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Email address of the shared mailbox
            string sharedMailbox = "shared@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || sharedMailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all item URIs from the shared mailbox's Inbox
                string[] itemUris = client.ListItems("Inbox", sharedMailbox);

                // Process up to the first 50 messages
                int messagesToProcess = Math.Min(50, itemUris.Length);
                for (int i = 0; i < messagesToProcess; i++)
                {
                    try
                    {
                        // Fetch each message by its URI
                        MailMessage message = client.FetchMessage(itemUris[i]);
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {i + 1}: {ex.Message}");
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
