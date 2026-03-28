using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the directory and file for logging
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "ews_log.txt");

            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Set up credentials for the Exchange server
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", credentials))
            {
                // Enable and configure logging via the base EmailClient class
                if (client is EmailClient emailClient)
                {
                    emailClient.EnableLogger = true;               // Turn on logging
                    emailClient.LogFileName = logFilePath;          // Destination file
                    emailClient.UseDateInLogFileName = false;      // Disable automatic date suffix
                }

                // Example operation: list messages in the Inbox folder
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(client.MailboxInfo.InboxUri);
                Console.WriteLine($"Inbox contains {inboxMessages.Count} messages.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
