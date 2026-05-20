using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Skip sending when placeholder credentials are detected.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Initialize Gmail client. The fourth parameter is a proxy; null means no proxy.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null))
            {
                // Create a simple mail message.
                using (MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is the email body."))
                {
                    // Add custom X-Mailing-Group header.
                    message.Headers.Add("X-Mailing-Group", "MarketingTeam");

                    // Send the message.
                    string messageId = gmailClient.SendMessage(message);
                    Console.WriteLine($"Message sent. Id: {messageId}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
