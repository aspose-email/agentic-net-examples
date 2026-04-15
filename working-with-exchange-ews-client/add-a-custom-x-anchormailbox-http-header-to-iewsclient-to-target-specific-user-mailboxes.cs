using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "username"; // placeholder
            string password = "password"; // placeholder

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(username) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(password) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);
            IEWSClient client = null;

            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
                // Add custom X-AnchorMailbox header
                client.AddHeader("X-AnchorMailbox", "user@example.com");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or configure EWS client: {ex.Message}");
                return;
            }

            // Create a simple email
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email";
            message.Body = "This is a test email sent via EWS with a custom header.";

            try
            {
                client.Send(message);
                Console.WriteLine("Message sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send message: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
