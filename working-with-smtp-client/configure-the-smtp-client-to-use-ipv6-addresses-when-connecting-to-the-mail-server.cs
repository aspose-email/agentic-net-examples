using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // IPv6 address of the SMTP server (example address)
            string smtpHost = "2001:0db8:85a3:0000:0000:8a2e:0370:7334";
            int smtpPort = 587; // Common submission port

            // Create the SMTP client with IPv6 host, port and automatic security negotiation
            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, SecurityOptions.Auto);

            // Set credentials (replace with real values)
            smtpClient.Username = "user@example.com";
            smtpClient.Password = "password";

            // Guard: skip sending when placeholder credentials are detected
            bool isPlaceholder = smtpClient.Username.Contains("example.com") && smtpClient.Password == "password";
            if (isPlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping actual send operation.");
                return;
            }

            // Prepare a simple email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("user@example.com");
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Test email over IPv6";
            message.Body = "This email was sent using an IPv6-enabled SMTP client.";

            // Send the message inside a using block to ensure proper disposal
            using (smtpClient)
            {
                smtpClient.Send(message);
                Console.WriteLine("Message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
