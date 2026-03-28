using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange WebDAV server URI and credentials
            string mailboxUri = "https://exchange.example.com/exchange";
            string username = "user@example.com";
            string password = "password";

            // Path to the attachment file
            string attachmentPath = "attachment.txt";

            // Verify that the attachment file exists before proceeding
            if (!File.Exists(attachmentPath))
            {
                Console.Error.WriteLine($"Attachment file not found: {attachmentPath}");
                return;
            }

            // Initialize the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Compose the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email via Aspose.Email";
                    message.Body = "Hello,\n\nThis is a test email sent using Aspose.Email Exchange WebDAV client.\n";

                    // Add the attachment
                    message.Attachments.Add(new Attachment(attachmentPath));

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
