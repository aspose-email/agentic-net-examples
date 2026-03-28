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
            string logFilePath = @"C:\Logs\EwsLog.txt";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Prepare connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Create the EWS client inside a using block
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Configure logging to the specified file
                client.LogFileName = logFilePath;

                // Example operation to verify the client works (list inbox messages)
                // This is optional and can be removed if only logging configuration is needed
                var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                Console.WriteLine($"Retrieved {messages.Count} messages from Inbox.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
