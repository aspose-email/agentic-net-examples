using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Primary and secondary SMTP server settings
            const string primaryHost = "smtp.primaryexample.com";
            const int primaryPort = 587;
            const string secondaryHost = "smtp.secondaryexample.com";
            const int secondaryPort = 587;
            const string username = "user@example.com";
            const string password = "password";

            // Guard against placeholder credentials to avoid real network calls in CI
            if (primaryHost.Contains("example.com") || secondaryHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP hosts detected. Skipping send operation.");
                return;
            }

            // Create a simple email message
            var message = new MailMessage
            {
                From = new MailAddress("sender@example.com"),
                Subject = "Test Email with Fallback",
                Body = "This email demonstrates fallback to an alternate SMTP host."
            };
            message.To.Add(new MailAddress("recipient@example.com"));

            // Attempt to send using the primary SMTP server
            bool sent = false;
            try
            {
                using (var client = new SmtpClient(primaryHost, primaryPort, username, password))
                {
                    client.Send(message);
                    Console.WriteLine("Email sent successfully via primary SMTP server.");
                    sent = true;
                }
            }
            catch (SmtpException ex)
            {
                Console.Error.WriteLine($"Primary SMTP server failed: {ex.Message}");
            }

            // Fallback to the secondary SMTP server if needed
            if (!sent)
            {
                try
                {
                    using (var client = new SmtpClient(secondaryHost, secondaryPort, username, password))
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully via secondary SMTP server.");
                    }
                }
                catch (SmtpException ex)
                {
                    Console.Error.WriteLine($"Secondary SMTP server also failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
