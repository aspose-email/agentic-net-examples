using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string csvPath = "recipients.csv";

            // Ensure CSV file exists; create a minimal placeholder if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    File.WriteAllText(csvPath, "Email,Name\nexample@example.com,John Doe");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV: {ex.Message}");
                    return;
                }
            }

            // Load recipient list from CSV
            List<MailMessage> messages = new List<MailMessage>();
            try
            {
                using (StreamReader reader = new StreamReader(csvPath))
                {
                    // Skip header line
                    string header = reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length < 2)
                            continue;

                        string email = parts[0].Trim();
                        string name = parts[1].Trim();

                        MailMessage message = new MailMessage();
                        message.From = "sender@example.com";
                        message.To.Add(email);
                        message.Subject = $"Hello {name}";
                        message.Body = $"Dear {name},\nThis is a personalized bulk email.";
                        messages.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV: {ex.Message}");
                return;
            }

            // SMTP client configuration (placeholders)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected. Skipping actual send.");
                return;
            }

            // Send bulk email
            try
            {
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    try
                    {
                        client.Send(messages);
                        Console.WriteLine("Bulk email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send emails: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect SMTP client: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
