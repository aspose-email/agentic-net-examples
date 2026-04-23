using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and message ID.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "YOUR_DEFAULT_EMAIL";
            string messageId = "YOUR_MESSAGE_ID";

            // If placeholders are not replaced, skip execution.
            if (accessToken.StartsWith("YOUR_") ||
                defaultEmail.StartsWith("YOUR_") ||
                messageId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials or message ID detected. Skipping Gmail call.");
                return;
            }

            // Create Gmail client.
            try
            {
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Fetch the specified message.
                    MailMessage message = null;
                    try
                    {
                        message = gmailClient.FetchMessage(messageId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch Gmail message: {ex.Message}");
                        return;
                    }

                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");

                        // Display content types of alternate views (MIME parts like HTML, plain text).
                        foreach (AlternateView view in message.AlternateViews)
                        {
                            Console.WriteLine($"Alternate view content type: {view.ContentType.MediaType}");
                        }

                        // Display content types of attachments.
                        foreach (Attachment attachment in message.Attachments)
                        {
                            Console.WriteLine($"Attachment content type: {attachment.ContentType.MediaType}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
