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
            // POP3 server connection settings (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Enable logging to a file
            string logFilePath = "pop3log.txt";

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                try
                {
                    // Retrieve mailbox information
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Message Count: {mailboxInfo.MessageCount}");
                    Console.WriteLine($"Occupied Size: {mailboxInfo.OccupiedSize}");

                    // List messages and display basic details
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Seq: {info.SequenceNumber}, Subject: {info.Subject}, Size: {info.Size}");
                    }
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }

            // After client disposal, read and display the log file if it exists
            if (File.Exists(logFilePath))
            {
                try
                {
                    string logContent = File.ReadAllText(logFilePath);
                    Console.WriteLine("\n--- POP3 Client Log ---");
                    Console.WriteLine(logContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read log file: {ex.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine("Log file was not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
