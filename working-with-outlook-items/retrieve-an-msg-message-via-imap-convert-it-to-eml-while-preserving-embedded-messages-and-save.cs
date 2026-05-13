using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI environments
            string imapHost = "imap.example.com";
            int imapPort = 993;
            string username = "user@example.com";
            string password = "password";

            if (imapHost.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                return;
            }

            // Output file path
            string outputPath = "message.eml";
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine($"Output directory does not exist: {outputDirectory}");
                return;
            }

            // Connect to IMAP server and fetch the first message
            using (ImapClient client = new ImapClient(imapHost, imapPort, username, password, SecurityOptions.SSLImplicit))
            {
                client.SelectFolder("INBOX");

                ImapMessageInfoCollection messages = client.ListMessages();
                if (messages == null || messages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in INBOX.");
                    return;
                }

                string messageUid = messages[0].UniqueId;
                MailMessage mailMessage = client.FetchMessage(messageUid);
                if (mailMessage == null)
                {
                    Console.Error.WriteLine("Failed to fetch the message.");
                    return;
                }

                // Save as EML while preserving embedded message formats
                EmlSaveOptions emlOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                mailMessage.Save(outputPath, emlOptions);
                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
