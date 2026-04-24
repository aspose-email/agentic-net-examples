using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configurable log file path (can be passed as first argument)
            string logFilePath = args.Length > 0 ? args[0] : "exchange_client.log";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(Path.GetFullPath(logFilePath));
            if (!Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                    return;
                }
            }

            // Placeholder connection settings – replace with real values when needed
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username   = "username";
            string password   = "password";

            // Guard against executing real network calls with placeholder credentials
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping actual server connection.");
                Console.WriteLine($"Logging would be written to: {logFilePath}");
                return;
            }

            // Create the Exchange WebDAV client inside a using block to ensure disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Enable server‑side logging
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true; // optional: include date in log file name

                // Attempt a safe operation to verify connectivity (list messages in Inbox)
                try
                {
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    int messageCount = messages != null ? messages.Count : 0;
                    Console.WriteLine($"Successfully connected. Inbox contains {messageCount} messages.");
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"Failed to communicate with Exchange server: {connEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
