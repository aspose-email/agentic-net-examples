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
            // Placeholder SMTP settings – skip actual network call in CI environments
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP settings detected. Skipping actual send.");
                return;
            }

            // Create SMTP client with custom timeout values
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                client.Timeout = 5000;            // 5 seconds for overall operations
                client.GreetingTimeout = 2000;    // 2 seconds for greeting phase

                // Prepare a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email";
                    message.Body = "Hello, this is a test.";

                    // Attempt to send the message with error handling
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Send failed: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
