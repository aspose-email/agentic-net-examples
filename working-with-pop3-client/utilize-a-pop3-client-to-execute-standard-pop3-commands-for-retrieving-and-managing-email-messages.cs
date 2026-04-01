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
            // Placeholder POP3 server credentials
            string host = "pop.example.com";
            string username = "user";
            string password = "pass";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || (username == "user" && password == "pass"))
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Ensure output directory exists before any file write
            string outputDirectory = "Output";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and connect POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    Console.WriteLine($"Total messages: {messages.Count}");

                    if (messages.Count > 0)
                    {
                        // Get first message info
                        Pop3MessageInfo firstInfo = messages[0];
                        Console.WriteLine($"Fetching message #{firstInfo.SequenceNumber}: {firstInfo.Subject}");

                        // Save the fetched message to a local .eml file
                        string outputPath = Path.Combine(outputDirectory, $"Message_{firstInfo.SequenceNumber}.eml");
                        client.SaveMessage(firstInfo.SequenceNumber, outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");

                        // Mark the message for deletion
                        client.DeleteMessage(firstInfo.SequenceNumber);
                        Console.WriteLine($"Message #{firstInfo.SequenceNumber} marked for deletion.");

                        // Commit deletions on the server
                        Console.WriteLine("Deletions committed.");
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
