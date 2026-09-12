using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server configuration (replace with real values)
                string host = "pop3.example.com";
                string username = "user@example.com";
                string password = "password";
                int port = 110;

                // Guard against placeholder credentials
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping operation.");
                    return;
                }

                // Ensure output directory exists
                string outputDir = "output";
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                    return;
                }

                // Create and use POP3 client
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
                {
                    pop3Client.SecurityOptions = SecurityOptions.Auto;

                    try
                    {
                        int messageCount = pop3Client.GetMessageCount();
                        Console.WriteLine($"Total messages on server: {messageCount}");

                        if (messageCount > 0)
                        {
                            // Fetch the first message (POP3 messages are 1-indexed)
                            using (MailMessage message = pop3Client.FetchMessage(1))
                            {
                                string filePath = Path.Combine(outputDir, "message1.eml");
                                try
                                {
                                    message.Save(filePath);
                                    Console.WriteLine($"Message saved to: {filePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages to retrieve.");
                        }
                    }
                    catch (Exception clientEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {clientEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
