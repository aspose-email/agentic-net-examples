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
            // Placeholder credentials – skip real network calls in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Prepare log file path and ensure its directory exists
            string logFilePath = Path.Combine(Environment.CurrentDirectory, "imap_activity.log");
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

            // Create and configure the IMAP client with detailed logging
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                try
                {
                    // Perform a simple operation to trigger connection and logging
                    client.Noop();
                    Console.WriteLine("IMAP client connected. Activity logging enabled.");
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
