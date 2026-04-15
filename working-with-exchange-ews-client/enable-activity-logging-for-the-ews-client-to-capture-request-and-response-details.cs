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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Enable activity logging
            string logPath = Path.Combine(Environment.CurrentDirectory, "ews_activity.log");
            try
            {
                string logDir = Path.GetDirectoryName(logPath);
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                client.LogFileName = logPath;
                client.UseDateInLogFileName = false; // optional: keep a single log file
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to configure logging: {ex.Message}");
                // Continue without logging
            }

            // Use the client (example: fetch mailbox info to generate log entries)
            try
            {
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine($"Mailbox display name: {mailboxInfo.MailboxUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
