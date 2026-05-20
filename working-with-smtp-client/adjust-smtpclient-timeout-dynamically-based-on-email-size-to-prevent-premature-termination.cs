using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration – replace with real values when running in production.
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                username.Contains("example.com", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create a simple mail message.
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Dynamic Timeout Example";
                message.Body = new string('A', 5000); // Simulated large body (5 KB).

                // Estimate message size in bytes (subject + body).
                int estimatedSize = System.Text.Encoding.UTF8.GetByteCount(message.Subject) +
                                    System.Text.Encoding.UTF8.GetByteCount(message.Body);

                // Determine timeout: base 10 seconds + 1 ms per byte (adjust as needed).
                int timeoutMilliseconds = 10_000 + estimatedSize;

                // Initialize the SMTP client and adjust its Timeout property.
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    try
                    {
                        client.Timeout = timeoutMilliseconds;
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
                        return;
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
