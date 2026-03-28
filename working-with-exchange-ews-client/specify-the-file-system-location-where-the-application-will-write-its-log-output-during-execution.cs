using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define log file location
            string logFilePath = Path.Combine(Environment.CurrentDirectory, "logs", "ews.log");

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Set up credentials (replace with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient("https://example.com/EWS/Exchange.asmx", credentials))
            {
                // Assign the log file path to the client
                client.LogFileName = logFilePath;

                // Example operation: list messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                Console.WriteLine($"Total messages in Inbox: {messages.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
