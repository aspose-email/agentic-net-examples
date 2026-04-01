using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real connection when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS client connection.");
                return;
            }

            // Prepare log file path and ensure the directory exists
            string logPath = Path.Combine(Environment.CurrentDirectory, "Logs", "EwsLog.txt");
            string logDir = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    client.LogFileName = logPath;
                    client.UseDateInLogFileName = true;

                    // Example operation to generate log entries
                    var mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Connected to mailbox: {mailboxInfo.MailboxUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
