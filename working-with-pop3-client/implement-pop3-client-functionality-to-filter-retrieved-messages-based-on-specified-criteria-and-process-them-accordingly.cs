using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Ensure the directory for saving attachments exists
            string outputDirectory = "DownloadedAttachments";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Initialize and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // Build a query to filter messages (e.g., subject contains "Invoice")
                    MailQuery query = new MailQuery("Subject Contains 'Invoice'");

                    // Retrieve messages that match the query
                    Pop3MessageInfoCollection messageInfos = client.ListMessages(query);

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Fetch the full message using its sequence number
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");

                            // Process and save each attachment
                            foreach (Attachment attachment in message.Attachments)
                            {
                                string attachmentPath = Path.Combine(outputDirectory, attachment.Name);
                                using (FileStream fileStream = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                                {
                                    attachment.ContentStream.CopyTo(fileStream);
                                }
                                Console.WriteLine($"Saved attachment: {attachment.Name}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
