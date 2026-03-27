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
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Log file path
            string logFilePath = "pop3_log.txt";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(Path.GetFullPath(logFilePath));
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Initialize and configure the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                client.EnableLogger = true;
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = false;

                // Trigger connection by retrieving the message count
                int messageCount = client.GetMessageCount();
                Console.WriteLine($"Message count: {messageCount}");

                // List messages and display basic information
                Pop3MessageInfoCollection messages = client.ListMessages();
                foreach (Pop3MessageInfo info in messages)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Size: {info.Size} bytes");
                }

                // Commit any pending deletions (none in this example)
                client.CommitDeletes();
            }

            // Read and output the POP3 client log
            if (File.Exists(logFilePath))
            {
                try
                {
                    string[] logLines = File.ReadAllLines(logFilePath);
                    Console.WriteLine("\n--- POP3 Client Log ---");
                    foreach (string line in logLines)
                    {
                        Console.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading log file: {ex.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine("Log file was not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
