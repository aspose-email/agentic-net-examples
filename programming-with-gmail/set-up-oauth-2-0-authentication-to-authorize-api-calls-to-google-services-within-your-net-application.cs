using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Clients;

namespace AsposeEmailGmailOAuthExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace the placeholders with your actual Google OAuth credentials.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                // Create the Gmail client using OAuth 2.0 credentials.
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    // Create a simple email message.
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = defaultEmail;
                        message.To = "recipient@example.com";
                        message.Subject = "Test Email via Aspose.Email Gmail OAuth";
                        message.Body = "This email was sent using Aspose.Email with OAuth 2.0 authentication.";

                        // Send the message and obtain the message Id.
                        string messageId = gmailClient.SendMessage(message);
                        Console.WriteLine("Message sent successfully. Id: " + messageId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}