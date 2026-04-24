using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI environments
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (string.IsNullOrWhiteSpace(host) ||
                host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(username) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("IMAP credentials are placeholders. Skipping execution.");
                return;
            }

            // Directory to store attachments
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Attachments");
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to IMAP server and process messages
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                // Validate connection by selecting the INBOX folder
                try
                {
                    await client.SelectFolderAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select INBOX: {ex.Message}");
                    return;
                }

                // Retrieve list of messages in INBOX
                ImapMessageInfoCollection messagesInfo;
                try
                {
                    messagesInfo = await client.ListMessagesAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                foreach (var info in messagesInfo)
                {
                    // Fetch the full message (including attachments)
                    MailMessage message;
                    try
                    {
                        message = await client.FetchMessageAsync(info.UniqueId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {info.UniqueId}: {ex.Message}");
                        continue;
                    }

                    using (message)
                    {
                        // Iterate over each attachment and save it preserving the original filename
                        foreach (Attachment attachment in message.Attachments)
                        {
                            string safeFileName = string.IsNullOrWhiteSpace(attachment.Name)
                                ? $"attachment_{Guid.NewGuid()}"
                                : attachment.Name;

                            string filePath = Path.Combine(outputDir, safeFileName);
                            try
                            {
                                attachment.Save(filePath);
                                Console.WriteLine($"Saved attachment: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {ex.Message}");
                            }
                        }
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
