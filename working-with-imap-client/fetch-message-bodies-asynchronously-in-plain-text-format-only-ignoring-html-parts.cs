using Aspose.Email.Clients;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                return;
            }

            // Ensure output directory exists
            string outputDir = "MessageBodies";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    await client.SelectFolderAsync("INBOX", null, CancellationToken.None);

                    // Retrieve list of messages in the folder
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync("INBOX", false);

                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message (plain‑text body will be available in MailMessage.Body)
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId);

                        // Use only the plain‑text body, ignore HTML parts
                        string plainBody = message.Body ?? string.Empty;

                        // Save the plain‑text body to a file named by the message UID
                        string filePath = Path.Combine(outputDir, $"{info.UniqueId}.txt");

                        try
                        {
                            await File.WriteAllTextAsync(filePath, plainBody);
                            Console.WriteLine($"Saved plain‑text body of message UID {info.UniqueId} to {filePath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to write file '{filePath}': {ioEx.Message}");
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
