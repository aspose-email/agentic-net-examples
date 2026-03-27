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
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the log file path
            string logFilePath = @"C:\Logs\exchange.log";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Exchange server URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client and configure logging
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = false; // optional: disable date suffix

                Console.WriteLine("Logging is configured. Log file: " + client.LogFileName);

                // Example operation to verify the client works (list inbox messages)
                try
                {
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    Console.WriteLine("Inbox contains " + messages.Count + " messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to list messages: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}