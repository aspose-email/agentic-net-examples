using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/hosts
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 host detected. Skipping network operations.");
                return;
            }

            // Directory where MSG files will be saved
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Pop3Messages");
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to POP3 server
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.ValidateCredentials();

                    // Retrieve list of messages
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Build a safe file name
                        string subject = string.IsNullOrWhiteSpace(info.Subject) ? "NoSubject" : info.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(c, '_');
                        }
                        string fileName = $"{subject}_{info.SequenceNumber}.msg";
                        string filePath = Path.Combine(outputDirectory, fileName);

                        // Save the message as MSG
                        try
                        {
                            client.SaveMessage(info.SequenceNumber, filePath);
                            Console.WriteLine($"Saved message {info.SequenceNumber} to \"{filePath}\"");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message {info.SequenceNumber}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
