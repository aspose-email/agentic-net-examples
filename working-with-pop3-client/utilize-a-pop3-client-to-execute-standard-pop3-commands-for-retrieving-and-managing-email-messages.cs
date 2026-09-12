using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients; // for SecurityOptions

// Author: Generated example demonstrating POP3 operations with Aspose.Email

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings
            string host = "pop3.example.com";
            int port = 110; // use 995 for SSL
            string username = "user@example.com";
            string password = "password";

            // Output directory for saved messages
            string outputDir = "Emails";


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

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                // Optional: set security options if needed (e.g., SSLImplicit)
                // client.SecurityOptions = SecurityOptions.SSLImplicit;

                // Get the total number of messages on the server
                int messageCount = client.GetMessageCount();

                Console.WriteLine($"Total messages on server: {messageCount}");

                // Retrieve each message, display its subject, and save to a file
                for (int i = 1; i <= messageCount; i++)
                {
                    try
                    {
                        using (MailMessage message = client.FetchMessage(i))
                        {
                            Console.WriteLine($"Message {i}: Subject = {message.Subject}");

                            string filePath = Path.Combine(outputDir, $"Message_{i}.eml");
                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved to {filePath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to save message {i}: {ioEx.Message}");
                            }
                        }
                    }
                    catch (Exception fetchEx)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {i}: {fetchEx.Message}");
                    }
                }

                // Example: delete the first message after processing
                if (messageCount > 0)
                {
                    try
                    {
                        client.DeleteMessage(1);
                        Console.WriteLine("Deleted message 1 from the server.");
                    }
                    catch (Exception delEx)
                    {
                        Console.Error.WriteLine($"Failed to delete message 1: {delEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
