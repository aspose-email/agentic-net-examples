using System;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP settings detected. Skipping email send.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Legacy client test";
                        message.Body = "This is a test email with ISO-8859-1 encoding.";
                        message.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
