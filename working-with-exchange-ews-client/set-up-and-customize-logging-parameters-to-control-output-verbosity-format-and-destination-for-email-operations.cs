using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection information
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Access logging properties via the EmailClient base class
                EmailClient emailClient = client as EmailClient;
                if (emailClient == null)
                {
                    Console.Error.WriteLine("Unable to access logging properties on the client.");
                    return;
                }

                // Enable logging
                emailClient.EnableLogger = true;

                // Prepare log directory and file
                string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
                try
                {
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {ex.Message}");
                    return;
                }

                // Set log file name and include date in the file name
                emailClient.LogFileName = Path.Combine(logDirectory, "ews_log.txt");
                emailClient.UseDateInLogFileName = true;

                // Example operation: retrieve mailbox information
                try
                {
                    var mailboxInfo = client.MailboxInfo;
                    Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
