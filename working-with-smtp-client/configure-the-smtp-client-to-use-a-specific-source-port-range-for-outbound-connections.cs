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
            // SMTP server configuration
            string host = "smtp.example.com";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            int remotePort = 587; // typical SMTP submission port

            // Create the SMTP client with the specified host and remote port
            using (SmtpClient smtpClient = new SmtpClient(host, remotePort))
            {
                // Set authentication credentials
                smtpClient.Username = "user@example.com";
                smtpClient.Password = "password";

                // Author note: Aspose.Email does not provide a direct API to set the local source port range
                // for outbound connections. This would need to be managed at the OS/network level
                // or via custom socket handling outside of Aspose.Email.

                // Prepare a simple email message
                MailMessage message = new MailMessage(
                    "user@example.com",
                    "recipient@example.com",
                    "Test Email",
                    "This is a test email sent using Aspose.Email SMTP client.");

                // Send the email
                smtpClient.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
