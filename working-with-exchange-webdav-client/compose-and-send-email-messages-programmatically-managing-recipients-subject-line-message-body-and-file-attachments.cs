using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values or the send operation will be skipped.
        string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
        string username   = "YOUR_USERNAME";
        string password   = "YOUR_PASSWORD";
        string domain     = "YOUR_DOMAIN";

        // Guard against placeholder credentials.
        if (username.Contains("YOUR_") || password.Contains("YOUR_") || domain.Contains("YOUR_"))
        {
            Console.Error.WriteLine("Placeholder credentials detected – email send operation skipped.");
            return;
        }

        // Create the email message.
        MailMessage message = new MailMessage
        {
            From    = "sender@example.com",
            To      = "recipient@example.com",
            Subject = "Sample Subject",
            Body    = "This is a sample email body."
        };

        // Create an in‑memory attachment to avoid file‑system dependencies.
        byte[] attachmentData = Encoding.UTF8.GetBytes("Sample attachment content.");
        using (MemoryStream ms = new MemoryStream(attachmentData))
        {
            // Ensure the stream is positioned at the beginning.
            ms.Position = 0;
            Attachment attachment = new Attachment(ms, "sample.txt");
            message.Attachments.Add(attachment);

            // Send the message via Exchange WebDav client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password, domain))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send message: {ex.Message}");
            }
        }
    }
}
