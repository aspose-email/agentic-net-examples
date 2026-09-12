using Aspose.Email;
using System;
using System.IO;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define log file path and ensure its directory exists
            string logFilePath = "EwsLog.txt";
            string fullLogPath = Path.GetFullPath(logFilePath);
            string logDirectory = Path.GetDirectoryName(fullLogPath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Prepare credentials for the Exchange server (placeholders)
            string user = "username";
            string pass = "password";
            string dom = "domain";

            // Guard: skip external calls when placeholders are used
            bool placeholdersInUse = string.Equals(user, "username", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(pass, "password", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(dom, "domain", StringComparison.OrdinalIgnoreCase);

            if (placeholdersInUse)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(user, pass, dom);

            // Initialize the EWS client (preserve variable name 'client')
            using (IEWSClient client = EWSClient.GetEWSClient("https://mail.example.com/EWS/Exchange.asmx", credentials))
            {
                // Activate comprehensive logging
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true;

                // Sample operation: retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
