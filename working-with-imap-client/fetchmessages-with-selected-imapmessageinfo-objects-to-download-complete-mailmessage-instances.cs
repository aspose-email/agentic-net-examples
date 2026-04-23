using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials check – skip execution if defaults are used.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and configure the IMAP client.
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                // Select the INBOX folder.
                client.SelectFolder("INBOX");

                // Retrieve a limited set of message identifiers (IdOnly) – e.g., first 5 messages.
                ImapMessageInfoCollection messageInfos = client.ListMessages("INBOX", ImapListFields.IdOnly, 5);

                // Ensure the output directory exists.
                string outputDir = "Output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Fetch each full message using its UniqueId and save it to a file.
                foreach (ImapMessageInfo info in messageInfos)
                {
                    try
                    {
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                            // Save the message; the .eml extension is inferred from the file name.
                            message.Save(filePath);
                            Console.WriteLine($"Saved message {info.UniqueId} to {filePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch or save message {info.UniqueId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
