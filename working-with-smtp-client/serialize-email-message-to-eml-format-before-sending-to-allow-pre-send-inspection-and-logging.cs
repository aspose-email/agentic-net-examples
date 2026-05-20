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
            // Placeholder SMTP configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual sending when placeholders are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Serialize the message to EML for inspection/logging
                string emlPath = "email.eml";
                try
                {
                    string directory = Path.GetDirectoryName(emlPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Use SaveOptions to specify EML format
                    message.Save(emlPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Message saved to {emlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save EML: {ex.Message}");
                    return;
                }

                // Send the message using SMTP client
                using (SmtpClient client = new SmtpClient(host, port))
                {
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto;

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
