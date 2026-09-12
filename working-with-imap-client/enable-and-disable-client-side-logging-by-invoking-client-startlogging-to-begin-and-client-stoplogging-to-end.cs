using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailLoggingDemo
{
    // Extension methods to provide StartLogging/StopLogging functionality
    public static class SmtpClientLoggingExtensions
    {
        public static void StartLogging(this SmtpClient client, string logFileName)
        {
            client.EnableLogger = true;
            client.LogFileName = logFileName;
        }

        public static void StopLogging(this SmtpClient client)
        {
            client.EnableLogger = false;
            client.LogFileName = null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Ensure the directory for the log file exists
                string logPath = "smtp_log.txt";
                string logDirectory = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Create the SMTP client (replace with valid host/port/credentials)
                SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password");

                // Guard: skip real network calls when placeholders are detected
                bool placeholders = client.Host.Contains("example.com") ||
                                    client.Username == "username" ||
                                    client.Password == "password";

                try
                {
                    // Enable client-side logging using the required API
                    client.StartLogging(logPath);

                    if (placeholders)
                    {
                        Console.WriteLine("Placeholder credentials detected – skipping actual email send.");
                    }
                    else
                    {
                        // Example operation: send a simple email
                        MailMessage message = new MailMessage
                        {
                            From = new MailAddress("sender@example.com"),
                            Subject = "Test Email",
                            Body = "This is a test email with logging enabled."
                        };
                        message.To.Add(new MailAddress("recipient@example.com"));

                        client.Send(message);
                    }

                    // Disable logging after the operation
                    client.StopLogging();
                }
                finally
                {
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
