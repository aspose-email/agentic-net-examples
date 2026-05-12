using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Create the email message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", ""))
            {
                // Ensure the body is treated as HTML
                message.IsBodyHtml = true;
                // Initial HTML content
                message.HtmlBody = "<p>Hello, this is the main content of the email.</p>";

                // Disclaimer footer to be appended
                const string disclaimer = "<p style='font-size:small;color:gray;'>This email is confidential and intended solely for the recipient.</p>";

                // Append the disclaimer to the existing HTML body
                message.HtmlBody += disclaimer;

                // Configure the SMTP client (placeholder values)
                try
                {
                    using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password"))
                    {
                        // Skip actual sending when placeholder credentials/host are detected
                        if (client.Host != null && client.Host.Contains("example.com"))
                        {
                            Console.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                            return;
                        }

                        // Send the email
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP client error: {ex.Message}");
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
