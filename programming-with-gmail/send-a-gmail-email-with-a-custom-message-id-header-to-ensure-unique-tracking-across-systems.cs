using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "your.email@example.com";

            // Guard against running with placeholder credentials.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") || defaultEmail.StartsWith("your."))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail send.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Compose the email.
                using (MailMessage message = new MailMessage())
                {
                    message.From = defaultEmail;
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test email with custom Message-Id";
                    message.Body = "This is a test email.";

                    // Add a custom Message-Id header.
                    message.Headers.Add("Message-ID", "<custom-id-12345@example.com>");

                    // Send the message.
                    string sentId = gmailClient.SendMessage(message);
                    Console.WriteLine("Message sent. Id: " + sentId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
