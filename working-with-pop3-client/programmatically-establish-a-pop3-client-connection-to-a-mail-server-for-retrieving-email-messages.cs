using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

// Author: Sample code demonstrating POP3 client usage with Aspose.Email
class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection details
            string host = "pop.example.com";
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                // Optional: set custom port or security options if required
                // client.Port = 110;
                // client.SecurityOptions = SecurityOptions.Auto;

                // Retrieve total number of messages on the server
                int messageCount = client.GetMessageCount();
                Console.WriteLine($"Total messages: {messageCount}");

                // Ensure output directory exists
                string outputDir = "RetrievedEmails";

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

                // Fetch each message and save as .eml file
                for (int i = 1; i <= messageCount; i++)
                {
                    try
                    {
                        MailMessage message = client.FetchMessage(i);
                        string filePath = Path.Combine(outputDir, $"Message_{i}.eml");
                        message.Save(filePath);
                        Console.WriteLine($"Saved message {i} to {filePath}");
                        message.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch/save message {i}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
