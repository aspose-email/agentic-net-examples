using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare log directory.
            string logDirectory = "Logs";
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating log directory: {dirEx.Message}");
                return;
            }

            // IMAP client configuration.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;
            string logFilePath = Path.Combine(logDirectory, "imap_log.txt");

            // Initialize and configure the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                try
                {
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false;

                    // Perform a simple operation to generate log entries.
                    client.SelectFolder("INBOX");
                    ImapMessageInfoCollection messages = client.ListMessages();
                    Console.WriteLine($"Total messages in INBOX: {messages.Count}");
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
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
