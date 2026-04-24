using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "your.email@gmail.com";

            // Guard against placeholder credentials to avoid live network calls.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("your."))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail send.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Compose the email message.
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = defaultEmail;
                        message.To.Add("recipient@example.com");
                        message.Subject = "High Priority Test";
                        message.Body = "This is a high priority email.";

                        // Set X-Priority header to indicate high priority.
                        message.Headers.Add("X-Priority", "1 (Highest)");
                        // Optionally set the MailPriority property.
                        message.Priority = MailPriority.High;

                        // Send the message via Gmail client.
                        string messageId = gmailClient.SendMessage(message);
                        Console.WriteLine("Message sent. Id: " + messageId);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error sending message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
