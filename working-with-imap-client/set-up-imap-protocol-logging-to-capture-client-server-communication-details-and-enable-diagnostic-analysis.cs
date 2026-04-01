using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip actual connection when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Ensure the directory for the log file exists
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "imap_log.txt");
            try
            {
                string logDir = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                // Wrap client operations in a try/catch to handle connection issues gracefully
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a simple query (e.g., fetch all messages)
                    MailQueryBuilder builder = new MailQueryBuilder();
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages using the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    Console.WriteLine($"Total messages retrieved: {messages.Count}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
