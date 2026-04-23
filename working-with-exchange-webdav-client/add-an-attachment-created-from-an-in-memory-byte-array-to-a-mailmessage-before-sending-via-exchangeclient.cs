using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip actual network call if defaults are used
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping sending email.");
                return;
            }

            // Create and configure the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Prepare in‑memory attachment data
                byte[] attachmentData = System.Text.Encoding.UTF8.GetBytes("This is the content of the attachment.");

                // Build the mail message with an attachment
                using (MemoryStream memoryStream = new MemoryStream(attachmentData))
                {
                    using (Attachment attachment = new Attachment(memoryStream, "attachment.txt"))
                    {
                        using (MailMessage message = new MailMessage())
                        {
                            message.From = "sender@example.com";
                            message.To = "receiver@example.com";
                            message.Subject = "Test email with attachment";
                            message.Body = "Please see the attached file.";

                            // Add the attachment to the message
                            message.Attachments.Add(attachment);

                            // Send the message via Exchange
                            client.Send(message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
