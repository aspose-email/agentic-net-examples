using System;
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
            string username = "user@example.com";
            string password = "password";

            // Skip sending when placeholder values are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Initialize the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com");

                    // Helper to add address only if it is not empty
                    void AddIfNotEmpty(MailAddressCollection collection, string address)
                    {
                        if (!string.IsNullOrWhiteSpace(address))
                        {
                            collection.Add(new MailAddress(address));
                        }
                    }

                    // Add recipients, filtering out empty entries
                    AddIfNotEmpty(message.To, "recipient1@example.com");
                    AddIfNotEmpty(message.To, ""); // empty, will be ignored
                    AddIfNotEmpty(message.Bcc, "recipient2@example.com");
                    // If you need CC, uncomment and add valid addresses
                    // AddIfNotEmpty(message.Cc, "cc@example.com");

                    message.Subject = "Test Email";
                    message.Body = "This is a test email.";

                    // Send the email
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
