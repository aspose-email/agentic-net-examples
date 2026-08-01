using Aspose.Email;
using System;
using System.IO;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Path for the detailed log file
            string logFilePath = "exchange_log.txt";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(Path.GetFullPath(logFilePath));
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Mailbox connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the WebDAV (Exchange) client and enable logging
            using (ExchangeClient client = new ExchangeClient(mailboxUri, credentials))
            {
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = false; // optional: keep a single log file

                // Perform a harmless operation to generate log entries (optional)
                // Example: accessing the MailboxUri property forces a request
                string uri = client.MailboxUri;

                // Additional operations can be placed here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
