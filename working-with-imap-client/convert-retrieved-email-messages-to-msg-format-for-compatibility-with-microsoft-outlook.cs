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
            // Placeholder IMAP server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputDir = "OutputMsg";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Retrieve list of messages
                ImapMessageInfoCollection messagesInfo = client.ListMessages();

                foreach (ImapMessageInfo info in messagesInfo)
                {
                    try
                    {
                        // Fetch the full message
                        MailMessage mail = client.FetchMessage(info.UniqueId);
                        using (mail)
                        {
                            // Convert to MAPI message
                            MapiMessage mapi = MapiMessage.FromMailMessage(mail);

                            // Save as MSG file
                            string msgPath = Path.Combine(outputDir, $"{info.UniqueId}.msg");
                            mapi.Save(msgPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process message {info.UniqueId}: {ex.Message}");
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
