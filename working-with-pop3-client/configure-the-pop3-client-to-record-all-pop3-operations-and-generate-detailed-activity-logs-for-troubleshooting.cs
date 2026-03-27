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
            // Prepare log file location
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "pop3_activity.log");

            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Initialize POP3 client with placeholder credentials
            using (Pop3Client client = new Pop3Client("pop3.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                // Enable detailed activity logging
                client.EnableLogger = true;
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true;

                try
                {
                    // Verify connection
                    client.Noop();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection test failed: {ex.Message}");
                    return;
                }

                try
                {
                    // Get total message count
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");

                    // List message metadata
                    Pop3MessageInfoCollection infos = client.ListMessages();
                    foreach (Pop3MessageInfo info in infos)
                    {
                        Console.WriteLine($"UID: {info.UniqueId}, Size: {info.Size} bytes");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
