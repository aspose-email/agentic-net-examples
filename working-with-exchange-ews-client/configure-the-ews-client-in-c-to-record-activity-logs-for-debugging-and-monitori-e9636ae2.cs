using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Path for the activity log file
                string logFilePath = "EwsClient.log";

                // Ensure the directory for the log file exists
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // EWS connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                NetworkCredential credentials = new NetworkCredential(username, password);

                // Create and configure the EWS client
                using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Enable activity logging
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = true;

                    // Perform a simple operation to generate log entries
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    Console.WriteLine("Fetched {0} messages.", messages.Count);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
            }
        }
    }
}