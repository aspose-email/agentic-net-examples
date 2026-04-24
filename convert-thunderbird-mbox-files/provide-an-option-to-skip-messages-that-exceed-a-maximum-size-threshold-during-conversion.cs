using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            long maxSizeBytes = 5 * 1024 * 1024; // 5 MB
            string outputFolder = "ConvertedMessages";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP configuration detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to IMAP server
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the Inbox folder
                    client.SelectFolder("INBOX");

                    // Get information about all messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Skip messages larger than the threshold
                        if (info.Size > maxSizeBytes)
                        {
                            Console.WriteLine($"Skipping message UID {info.UniqueId} (size {info.Size} bytes) exceeding threshold.");
                            continue;
                        }

                        // Fetch the message
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            // Save the message to a .eml file
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            string fileName = Path.Combine(outputFolder, $"{info.UniqueId}_{safeSubject}.eml");
                            try
                            {
                                message.Save(fileName, SaveOptions.DefaultEml);
                                Console.WriteLine($"Saved message UID {info.UniqueId} to {fileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message UID {info.UniqueId}: {ex.Message}");
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
