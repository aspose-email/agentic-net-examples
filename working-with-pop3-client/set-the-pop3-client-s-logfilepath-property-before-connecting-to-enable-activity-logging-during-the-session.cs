using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Log file path
            string logFilePath = "pop3log.txt";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the directory for the log file exists
            string logDir = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            // Create an empty log file if it does not exist
            if (!File.Exists(logFilePath))
            {
                using (File.Create(logFilePath)) { }
            }

            // Initialize POP3 client
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
            {
                // Enable activity logging
                pop3Client.LogFileName = logFilePath;

                // Perform a simple operation to trigger connection (e.g., get message count)
                try
                {
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Number of messages in mailbox: {messageCount}");
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
