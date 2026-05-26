using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials guard
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Ensure log directory exists (if a directory is specified)
            string logPath = "smtp.log";
            string logDirectory = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                client.SecurityOptions = SecurityOptions.Auto;
                client.EnableLogger = true;
                client.LogFileName = logPath;

                // Build the email message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email";
                message.Body = "Hello, this is a test email.";

                // Record payload size before sending
                long payloadSize = GetMessageSize(message);
                Console.WriteLine($"Email payload size: {payloadSize} bytes");

                // Send the message
                client.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    // Helper method to calculate the size of a MailMessage in bytes
    static long GetMessageSize(MailMessage message)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            // Save the message to the stream in EML format
            message.Save(stream, SaveOptions.DefaultEml);
            return stream.Length;
        }
    }
}
