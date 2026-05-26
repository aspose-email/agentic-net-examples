using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder SMTP server details
            string host = "smtp.example.com";
            int port = 587;
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping actual send operation.");
                return;
            }

            // Create the SMTP client (use SSLExplicit for STARTTLS)
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLExplicit))
            {
                // Enable pipelining mode
                client.UsePipelining = true;

                // Prepare first message
                MailMessage message1 = new MailMessage
                {
                    From = "sender@example.com",
                    Subject = "First Message",
                    Body = "This is the first test email."
                };
                message1.To.Add("recipient1@example.com");

                // Prepare second message
                MailMessage message2 = new MailMessage
                {
                    From = "sender@example.com",
                    Subject = "Second Message",
                    Body = "This is the second test email."
                };
                message2.To.Add("recipient2@example.com");

                // Send both messages using a collection (pipelining will batch the commands)
                MailMessageCollection messages = new MailMessageCollection { message1, message2 };
                client.Send(messages);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
