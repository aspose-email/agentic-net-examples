using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run against Gmail.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "your.email@example.com";

            // Skip external call when placeholders are detected.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("your."))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail fetch.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // List messages in the mailbox.
                    List<GmailMessageInfo> messagesInfo = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messagesInfo)
                    {
                        // Fetch the full message.
                        using (MailMessage message = gmailClient.FetchMessage(info.Id))
                        {
                            // Try to treat the message as an AMP message.
                            AmpMessage ampMessage = message as AmpMessage;
                            string plainText;

                            if (ampMessage != null && !string.IsNullOrEmpty(ampMessage.AmpHtmlBody))
                            {
                                // Convert AMP HTML body to plain text.
                                plainText = ampMessage.GetHtmlBodyText(true);
                            }
                            else
                            {
                                // Fallback to regular HTML body conversion.
                                plainText = message.GetHtmlBodyText(true);
                            }

                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine("Plain text content:");
                            Console.WriteLine(plainText);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
