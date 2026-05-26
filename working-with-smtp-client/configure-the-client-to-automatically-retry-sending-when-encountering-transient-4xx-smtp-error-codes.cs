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
            // SMTP server configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping send.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                client.SecurityOptions = SecurityOptions.Auto;

                // Retry settings for transient 4xx errors
                int maxAttempts = 3;
                int attempt = 0;
                bool sent = false;

                while (attempt < maxAttempts && !sent)
                {
                    attempt++;
                    try
                    {
                        // Prepare a simple email message
                        MailMessage message = new MailMessage();
                        message.From = username;
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test email";
                        message.Body = "This is a test.";

                        // Send the message
                        client.Send(message);
                        sent = true;
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (SmtpException ex) when (IsTransient4xx(ex.StatusCode))
                    {
                        Console.Error.WriteLine($"Transient SMTP error ({ex.StatusCode}) on attempt {attempt}. Retrying...");
                        if (attempt >= maxAttempts)
                        {
                            Console.Error.WriteLine("Maximum retry attempts reached. Giving up.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static bool IsTransient4xx(SmtpStatusCode statusCode)
    {
        // 4xx status codes are considered transient
        int code = (int)statusCode;
        return code >= 400 && code < 500;
    }
}
