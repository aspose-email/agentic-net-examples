using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid accidental network calls.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Gmail credentials are not set. Skipping execution.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Retrieve list of messages in the mailbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                foreach (GmailMessageInfo info in messages)
                {
                    // Fetch the full message for processing.
                    MailMessage message = gmailClient.FetchMessage(info.Id);

                    // ----- Process the message here -----
                    Console.WriteLine($"Processing message ID: {info.Id}, Subject: {message.Subject}");
                    // ------------------------------------

                    // Mark the message as read.
                    // Aspose.Email Gmail client does not expose a direct method to set the read flag.
                    // As a workaround, you can modify the message's label to remove "UNREAD".
                    // This requires using the underlying Google API which is not exposed here.
                    // Placeholder for read‑marking logic:
                    // gmailClient.ModifyMessageLabels(info.Id, removeLabels: new[] { "UNREAD" });
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
