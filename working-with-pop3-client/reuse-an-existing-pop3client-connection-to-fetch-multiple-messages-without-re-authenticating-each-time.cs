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
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping execution.");
                return;
            }

            // Prepare output directory for fetched messages
            string outputDir = "FetchedMessages";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Create and use a single POP3 client instance
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // Retrieve list of messages once
                    Pop3MessageInfoCollection messagesInfo = client.ListMessages();

                    foreach (Pop3MessageInfo messageInfo in messagesInfo)
                    {
                        // Fetch each message using the same client connection
                        using (MailMessage message = client.FetchMessage(messageInfo.SequenceNumber))
                        {
                            string filePath = Path.Combine(outputDir, $"Message_{messageInfo.SequenceNumber}.eml");
                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved message {messageInfo.SequenceNumber} to {filePath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message {messageInfo.SequenceNumber}: {saveEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
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
