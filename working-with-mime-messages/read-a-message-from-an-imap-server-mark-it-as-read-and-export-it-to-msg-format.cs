using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected.
            if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("IMAP connection parameters are placeholders. Skipping execution.");
                return;
            }

            // Ensure output directory exists.
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
            try
            {
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            string outputPath = Path.Combine(outputDir, "Message.msg");

            // Connect to IMAP server.
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages in the folder.
                    ImapMessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.Error.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Take the first message.
                    ImapMessageInfo messageInfo = messages[0];

                    // Mark the message as read.
                    client.AddMessageFlags(messageInfo.UniqueId, ImapMessageFlags.IsRead);

                    // Fetch the full message.
                    using (MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueId))
                    {
                        // Convert to MAPI message.
                        using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                        {
                            // Save as MSG.
                            try
                            {
                                mapiMessage.Save(outputPath);
                                Console.WriteLine($"Message saved to: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
