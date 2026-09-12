using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Output directory for downloaded messages
            string outputDir = "DownloadedMessages";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and connect the IMAP client (login performed in constructor)
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Select the INBOX folder
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception folderEx)
                {
                    Console.Error.WriteLine($"Failed to select folder: {folderEx.Message}");
                    return;
                }

                // Retrieve the list of messages in the selected folder
                ImapMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = client.ListMessages();
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                    return;
                }

                // Process each message
                foreach (ImapMessageInfo messageInfo in messageInfos)
                {
                    string filePath = Path.Combine(outputDir, $"{messageInfo.UniqueId}.eml");

                    // Download the message to a local file
                    try
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            client.SaveMessage(messageInfo.UniqueId, fileStream);
                        }
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message {messageInfo.UniqueId}: {saveEx.Message}");
                        continue;
                    }

                    // Apply a custom flag asynchronously to categorize the message
                    try
                    {
                        // Use a user‑defined flag name, e.g., "MyCustomFlag"
                        await client.AddMessageFlagsAsync(messageInfo.UniqueId, "MyCustomFlag", CancellationToken.None);
                    }
                    catch (Exception flagEx)
                    {
                        Console.Error.WriteLine($"Failed to add custom flag to message {messageInfo.UniqueId}: {flagEx.Message}");
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
