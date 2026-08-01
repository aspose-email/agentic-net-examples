using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

// Author: Example POP3 client using Aspose.Email
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection settings
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Ensure output directory exists
            string outputDir = "Output";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and configure POP3 client
            using (Pop3Client pop3Client = new Pop3Client())
            {
                try
                {
                    pop3Client.Host = host;
                    pop3Client.Port = port;
                    pop3Client.Username = username;
                    pop3Client.Password = password;
                    pop3Client.SecurityOptions = SecurityOptions.Auto;

                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = pop3Client.ListMessages();
                    Console.WriteLine($"Total messages: {messages.Count}");

                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"ID: {info.UniqueId}, Subject: {info.Subject}");
                    }

                    // Retrieve the first message if any exist
                    if (messages.Count > 0)
                    {
                        Pop3MessageInfo firstInfo = messages[0];
                        MailMessage message = pop3Client.FetchMessage(firstInfo.UniqueId);
                        string outputPath = Path.Combine(outputDir, $"Message_{firstInfo.UniqueId}.eml");

                        // Save the retrieved email to a file
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");

                        // Dispose the MailMessage
                        message.Dispose();
                    }
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
