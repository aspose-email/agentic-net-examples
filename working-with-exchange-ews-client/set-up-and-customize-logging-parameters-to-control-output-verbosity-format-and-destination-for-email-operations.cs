using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define log file path
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "ews_log.txt");

            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create credentials (replace with real values)
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Configure logging
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true; // Include date in log file name

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}