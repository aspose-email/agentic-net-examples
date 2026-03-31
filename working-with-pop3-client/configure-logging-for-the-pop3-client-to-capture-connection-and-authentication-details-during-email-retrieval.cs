using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder POP3 server credentials
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 connection.");
                return;
            }

            // Prepare log file path and ensure its directory exists
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logPath = Path.Combine(logDirectory, "pop3log.txt");
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                return;
            }

            // Initialize POP3 client with logging enabled
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                client.EnableLogger = true;
                client.LogFileName = logPath;
                client.UseDateInLogFileName = true;

                // Attempt to validate credentials (wrapped in try/catch for safety)
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine($"Authentication failed: {credEx.Message}");
                    return;
                }

                // Retrieve and display message count
                try
                {
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages in mailbox: {messageCount}");
                }
                catch (Exception fetchEx)
                {
                    Console.Error.WriteLine($"Failed to retrieve messages: {fetchEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
