using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace Pop3ActivityLoggingSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server credentials (replace with real values)
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls during CI
                if (host.Contains("example.com") || (username == "username" && password == "password"))
                {
                    Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                    return;
                }

                // Prepare log file path
                string logPath = "pop3log.txt";
                try
                {
                    string logDirectory = Path.GetDirectoryName(logPath);
                    if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare log directory: {dirEx.Message}");
                    return;
                }

                // Initialize POP3 client with logging enabled
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    client.EnableLogger = true;
                    client.LogFileName = logPath;
                    client.UseDateInLogFileName = true;

                    try
                    {
                        // Retrieve mailbox status
                        Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Message count: {mailboxInfo.MessageCount}");
                        Console.WriteLine($"Occupied size: {mailboxInfo.OccupiedSize}");

                        // List messages to generate detailed activity logs
                        Pop3MessageInfoCollection messages = client.ListMessages();
                        Console.WriteLine($"Listed {messages.Count} messages.");
                    }
                    catch (Pop3Exception popEx)
                    {
                        Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                        return;
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
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
