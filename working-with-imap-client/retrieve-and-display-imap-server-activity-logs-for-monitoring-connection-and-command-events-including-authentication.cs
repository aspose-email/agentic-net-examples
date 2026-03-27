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
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Log file path
            string logFilePath = "imap_log.txt";

            // Prepare log file (delete if exists)
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log file: {ex.Message}");
                return;
            }

            // Create and use ImapClient
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Enable logging
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Execute a simple command to generate log entries
                    client.Noop();

                    // Disable logging and reset settings
                    client.EnableLogger = false;
                    client.ResetLogSettings();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                return;
            }

            // Read and display the log file
            try
            {
                if (!File.Exists(logFilePath))
                {
                    Console.Error.WriteLine("Log file was not created.");
                    return;
                }

                using (FileStream logStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(logStream))
                {
                    Console.WriteLine("IMAP Server Activity Log:");
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading log file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}