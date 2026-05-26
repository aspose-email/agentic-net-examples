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
            // Define SMTP connection parameters (placeholders)
            string host = "smtp.example.com";
            int port = 25;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP host detected. Skipping actual send operation.");
                return;
            }

            // Prepare log file path and ensure its directory exists
            string logFilePath = Path.Combine(Environment.CurrentDirectory, "smtp_log.txt");
            try
            {
                string logDir = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Enable internal logger to capture server responses
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                // Optional: subscribe to OnConnect event for additional logging
                client.OnConnect += (sender, args) =>
                {
                    Console.WriteLine("Connected to SMTP server.");
                };

                // Create a simple email message
                MailMessage message = new MailMessage
                {
                    From = username,
                    To = "recipient@example.com",
                    Subject = "Test Email",
                    Body = "This is a test email sent using Aspose.Email."
                };

                // Send the message
                try
                {
                    client.Send(message);
                }
                catch (Exception sendEx)
                {
                    Console.Error.WriteLine($"Error during send: {sendEx.Message}");
                    return;
                }

                // After sending, read and output the logged server responses
                try
                {
                    if (File.Exists(logFilePath))
                    {
                        Console.WriteLine("SMTP server responses:");
                        foreach (string line in File.ReadAllLines(logFilePath))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Log file not found; no server responses captured.");
                    }
                }
                catch (Exception logEx)
                {
                    Console.Error.WriteLine($"Failed to read log file: {logEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
