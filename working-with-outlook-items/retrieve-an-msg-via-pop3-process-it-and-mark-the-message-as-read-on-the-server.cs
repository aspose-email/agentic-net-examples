using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                return;
            }

            // Output file path for the retrieved MSG
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            string outputPath = Path.Combine(outputDir, "message.msg");

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to POP3 server and retrieve the first message
            using (Pop3Client client = new Pop3Client(host, port, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 authentication failed: {ex.Message}");
                    return;
                }

                // Get list of messages
                Pop3MessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messages == null || messages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found on the POP3 server.");
                    return;
                }

                // Retrieve the first message (by sequence number)
                Pop3MessageInfo firstInfo = messages[0];
                MailMessage mailMessage;
                try
                {
                    mailMessage = client.FetchMessage(firstInfo.SequenceNumber);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // Simple processing: display subject
                    Console.WriteLine($"Subject: {mailMessage.Subject}");

                    // Convert to MAPI message and save as MSG
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);
                    using (mapiMessage)
                    {
                        try
                        {
                            mapiMessage.Save(outputPath);
                            Console.WriteLine($"Message saved as MSG to: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                        }
                    }

                    // Mark as read – POP3 does not support read flags, so we delete the message after processing
                    try
                    {
                        client.DeleteMessage(firstInfo.SequenceNumber);
                        Console.WriteLine("Message marked as read (deleted) on the server.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete (mark as read) message: {ex.Message}");
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
