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
            // Connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Enable detailed SOAP tracing
                string logPath = "EwsLog.txt";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string logDir = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                client.LogFileName = logPath;
                client.ReturnClientRequestId = true; // optional for extra detail

                // Perform a simple operation to generate SOAP traffic
                try
                {
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    Console.WriteLine($"Retrieved {messages.Count} messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
