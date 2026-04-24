using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string logFilePath = "imap_log.txt";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Ensure the directory for the log file exists
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                try
                {
                    // Perform a lightweight operation to trigger connection and logging
                    client.SelectFolder("INBOX");
                    Console.WriteLine("IMAP connection established and activity logged.");
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
