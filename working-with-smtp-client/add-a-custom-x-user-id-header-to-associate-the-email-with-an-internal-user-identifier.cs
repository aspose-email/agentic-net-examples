using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string userEmail = "user@example.com";
            string accessToken = "PLACEHOLDER_ACCESS_TOKEN";

            // Guard against placeholder credentials to avoid unwanted network calls
            if (userEmail.Contains("example.com") ||
                string.IsNullOrWhiteSpace(accessToken) ||
                accessToken.StartsWith("PLACEHOLDER"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Initialize Gmail client (IGmailClient) using the static factory method
            using (IGmailClient gmailClient = GmailClient.GetInstance(userEmail, accessToken))
            {
                // Create a new mail message
                MailMessage message = new MailMessage();
                message.From = userEmail;
                message.To.Add("recipient@example.org");
                message.Subject = "Test Email with Custom Header";
                message.Body = "This email contains a custom X-User-Id header.";

                // Add custom X-User-Id header
                message.Headers.Add("X-User-Id", "12345");

                // Send the message and capture the returned message Id
                try
                {
                    string messageId = gmailClient.SendMessage(message);
                    Console.WriteLine("Message sent successfully. Id: " + messageId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to send message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
