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
            // Define SMTP server details (placeholders)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping actual send operation.");
                return;
            }

            // Prepare log file path and ensure its directory exists
            string logFilePath = "smtp_log.txt";
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ioEx.Message}");
                return;
            }

            // Create a simple email message
            using (MailMessage message = new MailMessage())
            {
                message.From = username;
                message.To.Add(username);
                message.Subject = "Test Email";
                message.Body = "This is a test email for SMTP transaction logging.";

                // Initialize the SMTP client with logging enabled
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Optional: subscribe to connection event for additional console output
                    client.OnConnect += (sender, args) =>
                    {
                        Console.WriteLine($"Connected to SMTP server {host}:{port}");
                    };

                    try
                    {
                        // Validate credentials before sending
                        client.ValidateCredentials();

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (SmtpException smtpEx)
                    {
                        Console.Error.WriteLine($"SMTP error ({smtpEx.StatusCode}): {smtpEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
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
