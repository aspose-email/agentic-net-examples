using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server details (placeholders)
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network call.");
                return;
            }

            // Prepare output directory and file path
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "message.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Connect to POP3 server
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate POP3 credentials: {ex.Message}");
                    return;
                }

                // Get the number of messages in the mailbox
                int messageCount = client.GetMessageCount();
                if (messageCount == 0)
                {
                    Console.Error.WriteLine("No messages found in the mailbox.");
                    return;
                }

                // Retrieve the first message (sequence number 1)
                using (MailMessage message = client.FetchMessage(1))
                {
                    try
                    {
                        // Save the message as MSG format
                        message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
