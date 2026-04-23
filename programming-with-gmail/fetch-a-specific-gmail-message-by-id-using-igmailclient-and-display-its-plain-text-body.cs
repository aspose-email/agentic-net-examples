using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string messageId = "YOUR_MESSAGE_ID";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN" ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail fetch.");
                return;
            }

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Fetch the message
                    using (MailMessage message = gmailClient.FetchMessage(messageId))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("Body:");
                        Console.WriteLine(message.Body);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error fetching Gmail message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
