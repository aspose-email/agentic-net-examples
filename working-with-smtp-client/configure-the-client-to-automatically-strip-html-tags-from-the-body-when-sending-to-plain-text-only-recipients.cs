using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    // Prepare a mail message with HTML content
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Message with HTML";
                        message.IsBodyHtml = true;
                        message.HtmlBody = "<h1>Hello World</h1><p>This is a <b>test</b> email.</p>";

                        // Simulate detection of a plain‑text only recipient
                        bool recipientSupportsOnlyPlainText = true; // In real scenarios, determine this via recipient capabilities

                        if (recipientSupportsOnlyPlainText && message.IsBodyHtml)
                        {
                            // Strip HTML tags by converting the HTML body to plain text
                            string plainText = message.GetHtmlBodyText(true);
                            message.Body = plainText;
                            message.IsBodyHtml = false;
                            message.HtmlBody = null;
                        }

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during send operation: {ex.Message}");
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
