using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define POP3 connection parameters
            string host = "pop.example.com";
            string username = "user";
            string password = "password";

            // Define log file path
            string logFilePath = "logs/pop3log.txt";

            // Ensure the log directory exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                    return;
                }
            }

            // Create and configure the POP3 client
            try
            {
                using (Pop3Client client = new Pop3Client(host, username, password))
                {
                    // Set the log file name before any operation to enable logging
                    client.LogFileName = logFilePath;

                    // Optionally enable the logger (default is true when LogFileName is set)
                    client.EnableLogger = true;

                    // Retrieve the list of messages (this will establish the connection)
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages retrieved: {messages.Count}");
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
