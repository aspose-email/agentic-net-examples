using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server details – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                return;
            }

            // Create and dispose the IMAP client safely.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                // Ensure the client can access the target folder.
                try
                {
                    await client.SelectFolderAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Locate archived .eml files.
                string archiveFolder = "ArchivedEmails";
                if (!Directory.Exists(archiveFolder))
                {
                    Console.Error.WriteLine($"Archive folder not found: {archiveFolder}");
                    return;
                }

                string[] emlFiles = Directory.GetFiles(archiveFolder, "*.eml");
                List<MailMessage> messages = new List<MailMessage>();

                // Load each .eml file into a MailMessage instance.
                foreach (string filePath in emlFiles)
                {
                    if (!File.Exists(filePath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                        try
                        {
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                            return;
                        }

                        Console.Error.WriteLine($"File not found: {filePath}");
                        continue;
                    }

                    try
                    {
                        MailMessage message = MailMessage.Load(filePath);
                        messages.Add(message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load EML file '{filePath}': {ex.Message}");
                    }
                }

                if (messages.Count == 0)
                {
                    Console.WriteLine("No messages to append.");
                    return;
                }

                // Append the loaded messages asynchronously.
                try
                {
                    AppendMessagesResult result = await client.AppendMessagesAsync(messages);
                    Console.WriteLine($"Successfully appended {messages.Count} message(s) to the mailbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
