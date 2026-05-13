using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            if (string.IsNullOrWhiteSpace(clientId) ||
                clientId.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                clientSecret.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                refreshToken.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping send operation.");
                return;
            }

            // Create Gmail client. Use null for proxy (no proxy).
            IGmailClient gmailClient;
            try
            {
                // Overload: GetInstance(string clientId, IWebProxy proxy, string clientSecret, string refreshToken)
                gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Prepare the email message.
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("sender@example.com");
                msg.Subject = "Test Email with Tracking Headers";
                msg.Body = "This email contains custom X-Tracking-Id headers for each recipient.";

                // Define recipients using MailAddressCollection.
                MailAddressCollection recipients = new MailAddressCollection
                {
                    new MailAddress("alice@example.com"),
                    new MailAddress("bob@example.com")
                };

                foreach (MailAddress recipient in recipients)
                {
                    msg.To.Add(recipient);

                    // Insert a unique tracking header for this recipient.
                    string headerName = $"X-Tracking-Id-{recipient.Address}";
                    string headerValue = Guid.NewGuid().ToString();
                    msg.Headers.Add(headerName, headerValue);
                }

                // Send the message.
                try
                {
                    string messageId = gmailClient.SendMessage(msg);
                    Console.WriteLine($"Message sent successfully. Id: {messageId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }

            // Dispose the client if it implements IDisposable.
            if (gmailClient is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
