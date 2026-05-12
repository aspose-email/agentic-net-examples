using Aspose.Email.Clients;
using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        // IMAP server configuration (replace with real values or keep placeholders)
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";
        string folderName = "INBOX";
        string outputDirectory = "DownloadedMessages";

        // Guard: skip network operations when placeholder values are detected
        if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.WriteLine("Placeholder configuration detected. Skipping IMAP operations.");
            return;
        }

        // Ensure the output directory exists
        try
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        // Connect to the IMAP server
        using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
        {
            try
            {
                // Select the target folder
                client.SelectFolder(folderName);

                // Retrieve the list of messages in the folder
                ImapMessageInfoCollection messages = client.ListMessages();

                foreach (ImapMessageInfo messageInfo in messages)
                {
                    try
                    {
                        // Asynchronously fetch the full message
                        MailMessage message = await client.FetchMessageAsync(messageInfo.UniqueId);

                        // Save the message to a local .eml file
                        string filePath = Path.Combine(outputDirectory, $"{messageInfo.UniqueId}.eml");
                        try
                        {
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                message.Save(fs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message {messageInfo.UniqueId}: {ex.Message}");
                            continue;
                        }

                        // Apply a custom label by setting a flag (using Flagged as an example)
                        await client.AddMessageFlagsAsync(messageInfo.UniqueId, ImapMessageFlags.Flagged);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message {messageInfo.UniqueId}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
            }
        }
    }
}
