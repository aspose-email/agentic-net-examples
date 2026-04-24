using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using System.Net.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "your.email@example.com";

            if (accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail send operation.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            // Build the email message.
            MailMessage message = new MailMessage();
            message.From = defaultEmail;
            message.To.Add("recipient@example.com");
            message.Subject = "Sample multipart/alternative email";

            // Plain‑text part.
            string plainText = "This is the plain‑text version of the email.";
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainText, null, MediaTypeNames.Text.Plain);
            message.AlternateViews.Add(plainView);

            // HTML part.
            string htmlText = "<html><body><h1>Hello!</h1><p>This is the <b>HTML</b> version of the email.</p></body></html>";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlText, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(htmlView);

            // Send the message via Gmail.
            try
            {
                string sentMessageId = gmailClient.SendMessage(message);
                Console.WriteLine($"Message sent successfully. Id: {sentMessageId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                // Dispose the client and message.
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
