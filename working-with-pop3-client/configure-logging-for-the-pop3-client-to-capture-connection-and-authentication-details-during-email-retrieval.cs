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
            // POP3 server configuration
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Log file configuration
            string logFilePath = "pop3log.txt";
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Enable logging
                client.EnableLogger = true;
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = false;

                // Optional: capture connection event
                client.OnConnect += (sender, e) =>
                {
                    Console.WriteLine("Connected to POP3 server.");
                };

                // Perform a simple operation to trigger connection and authentication
                try
                {
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operations: {ex.Message}");
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
