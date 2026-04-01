using System;
using System.IO;
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
            // Placeholder credentials – skip real network call in CI environments
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping POP3 connection due to placeholder credentials.");
                return;
            }

            // Connect to POP3 server safely
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate connection and retrieve mailbox status
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Mailbox contains {mailboxInfo.MessageCount} messages, occupying {mailboxInfo.OccupiedSize} bytes.");

                    if (mailboxInfo.MessageCount == 0)
                    {
                        Console.WriteLine("No messages to process.");
                        return;
                    }

                    // List messages
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    Pop3MessageInfo firstMessageInfo = messages[0];
                    Console.WriteLine($"First message subject: {firstMessageInfo.Subject}");

                    // Fetch the first message
                    using (MailMessage message = client.FetchMessage(firstMessageInfo.SequenceNumber))
                    {
                        // Prepare output path
                        string outputPath = "message.eml";
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the message to file safely
                        try
                        {
                            message.Save(outputPath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Message saved to {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
