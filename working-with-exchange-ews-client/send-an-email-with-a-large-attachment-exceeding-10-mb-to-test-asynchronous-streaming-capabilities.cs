using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the large attachment
            const string attachmentPath = "largeAttachment.bin";

            // Ensure the attachment file exists and is larger than 10 MB
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    const long sizeInBytes = 11L * 1024 * 1024; // 11 MB
                    using (FileStream fs = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        fs.SetLength(sizeInBytes);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test large attachment",
                "Please see the attached large file."))
            {
                // Add the large attachment
                try
                {
                    message.Attachments.Add(new Attachment(attachmentPath));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Initialize the EWS client
                IEWSClient client = null;
                try
                {
                    string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                    string username = "username";
                    string password = "password";

                    client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                    return;
                }

                // Send the message using the EWS client
                try
                {
                    using (client)
                    {
                        client.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
