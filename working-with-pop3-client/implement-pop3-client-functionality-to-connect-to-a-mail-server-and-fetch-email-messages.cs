using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

// Author: Generated example demonstrating POP3 client usage with Aspose.Email

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

            // Directory to save fetched emails
            string outputDir = "FetchedEmails";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and configure the POP3 client
            using (Pop3Client pop3Client = new Pop3Client())
            {
                pop3Client.Host = host;
                pop3Client.Port = port;
                pop3Client.Username = username;
                pop3Client.Password = password;
                pop3Client.SecurityOptions = SecurityOptions.Auto; // Auto-detect SSL/TLS

                try
                {
                    // Retrieve the list of messages on the server
                    Pop3MessageInfoCollection messages = pop3Client.ListMessages();

                    Console.WriteLine($"Total messages on server: {messages.Count}");

                    // Iterate through each message and save it locally
                    foreach (Pop3MessageInfo info in messages)
                    {
                        try
                        {
                            // Fetch the full message by its unique identifier
                            MailMessage message = pop3Client.FetchMessage(info.UniqueId);

                            // Prepare a safe filename based on subject
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }

                            string filePath = Path.Combine(outputDir, $"{info.UniqueId}_{safeSubject}.eml");

                            // Save the message to an .eml file
                            message.Save(filePath);
                            Console.WriteLine($"Saved message {info.UniqueId} to {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch or save message {info.UniqueId}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
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
