using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Path for the activity log file
            string logPath = "imap_activity.log";

            // Ensure the log file directory exists
            try
            {
                string logDirectory = Path.GetDirectoryName(Path.GetFullPath(logPath));
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {dirEx.Message}");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                // Enable activity logging
                client.EnableLogger = true;
                client.LogFileName = logPath;

                // Attempt to connect and authenticate
                try
                {
                    client.Noop(); // Triggers connection and authentication
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during IMAP connection: {ex.Message}");
                    return;
                }
            }

            // Read and display the activity log
            try
            {
                if (File.Exists(logPath))
                {
                    string[] logLines = File.ReadAllLines(logPath);
                    Console.WriteLine("=== IMAP Activity Log ===");
                    foreach (string line in logLines)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Log file was not created.");
                }
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine($"Failed to read log file: {logEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
