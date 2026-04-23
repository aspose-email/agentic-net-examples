using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Email details
            string fromAddress = "sender@example.com";
            string toAddress = "recipient@example.com";
            string subject = "Important: Please read";
            string body = "This is a high priority email with a read receipt request.";

            // SMTP server configuration (placeholder values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Guard against placeholder SMTP host to avoid real network calls
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create the mail message
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = body;

            // Set high priority
            message.Priority = MailPriority.High;

            // Request read receipt
            message.ReadReceiptTo = new MailAddress(fromAddress);

            // Send the message using SmtpClient
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Username = smtpUser;
                client.Password = smtpPass;
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
