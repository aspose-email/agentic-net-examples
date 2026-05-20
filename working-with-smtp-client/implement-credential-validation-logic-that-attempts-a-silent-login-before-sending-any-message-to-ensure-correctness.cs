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
            // Define SMTP server settings (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls during CI
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping email send.");
                return;
            }

            // Create the SMTP client with explicit variable name 'client'
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Attempt silent login to validate credentials
                    bool credentialsValid = client.ValidateCredentials();
                    if (!credentialsValid)
                    {
                        Console.Error.WriteLine("SMTP authentication failed. Check credentials.");
                        return;
                    }

                    // Prepare a simple email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = username;
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email";
                        message.Body = "This is a test email sent after credential validation.";

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
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
