using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailLogExample
{
    // Author: Aspose.Email example author
    class Program
    {
        static void Main()
        {
            try
            {
                // Define log file location
                string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AsposeEmailLogs", "email.log");

                // Ensure the directory for the log file exists
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Initialize EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Set the log file name for diagnostics
                    client.LogFileName = logFilePath;

                    // Example operation: retrieve mailbox info (optional)
                    // var mailboxInfo = client.GetMailboxInfo();
                    // Console.WriteLine("Mailbox info retrieved successfully.");
                }

                Console.WriteLine("Log file location set to: " + logFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
