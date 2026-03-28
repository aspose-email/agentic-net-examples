using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

public class Program
{
    public static void Main()
    {
        try
        {
            // Define log file path and ensure its directory exists
            string logFilePath = "pop3_log.txt";
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Initialize POP3 client with required credentials
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = "pop.example.com";
                client.Username = "user@example.com";
                client.Password = "password";
                client.SecurityOptions = SecurityOptions.Auto;

                // Enable detailed activity logging
                client.EnableLogger = true;
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = true;

                try
                {
                    // Perform a simple operation to generate logs
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");
                }
                catch (Exception ex)
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
