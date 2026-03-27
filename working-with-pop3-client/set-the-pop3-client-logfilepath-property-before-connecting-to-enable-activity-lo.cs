using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define POP3 server credentials
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Define log file path
            string logFilePath = "pop3_log.txt";

            // Ensure the directory for the log file exists
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating log directory: {dirEx.Message}");
                return;
            }

            // Create and configure the POP3 client
            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Perform a simple operation to trigger connection and logging
                    try
                    {
                        var mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Total messages: {mailboxInfo.MessageCount}");
                    }
                    catch (Exception opEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {opEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to initialize POP3 client: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
