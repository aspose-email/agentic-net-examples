using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Destination folder for extracted attachments
            string outputFolder = "Attachments";

            // Guard against placeholder credentials to avoid live network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operations.");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create and connect the POP3 client (constructor performs connection)
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Retrieve the list of messages
                Pop3MessageInfoCollection messageInfos = client.ListMessages();

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    // Fetch the full message
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Iterate through each attachment in the message
                        foreach (Attachment attachment in message.Attachments)
                        {
                            string attachmentPath = Path.Combine(outputFolder, attachment.Name ?? "unnamed_attachment");

                            // Save the attachment content to a file
                            try
                            {
                                using (FileStream fileStream = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                                {
                                    attachment.ContentStream.CopyTo(fileStream);
                                }
                                Console.WriteLine($"Saved attachment: {attachmentPath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {ioEx.Message}");
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
