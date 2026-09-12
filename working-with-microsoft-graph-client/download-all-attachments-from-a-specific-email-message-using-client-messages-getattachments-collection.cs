using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings for the IMAP server
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create the IMAP client with the variable name 'client'
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Sequence number of the email message whose attachments we want to download
                int messageSequenceNumber = 1; // adjust as needed

                // Retrieve the list of attachment information for the specified message
                ImapAttachmentInfoCollection attachmentInfos = client.ListAttachments(messageSequenceNumber);

                // Ensure the output directory exists
                string outputDirectory = "Attachments";

                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Iterate through each attachment info and download the attachment
                foreach (ImapAttachmentInfo info in attachmentInfos)
                {
                    // Fetch the attachment using its name (since Id property does not exist)
                    Attachment attachment = client.FetchAttachment(messageSequenceNumber, info.Name);

                    // Build the full file path for saving
                    string filePath = Path.Combine(outputDirectory, info.Name);

                    // Save the attachment to disk
                    attachment.Save(filePath);
                    attachment.Dispose();

                    Console.WriteLine($"Saved attachment: {filePath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
