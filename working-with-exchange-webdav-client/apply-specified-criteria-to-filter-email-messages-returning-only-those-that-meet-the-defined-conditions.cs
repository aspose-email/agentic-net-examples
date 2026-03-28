using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Initialize Gmail client with placeholder credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // Retrieve the list of messages in the mailbox
                List<GmailMessageInfo> messageInfos = gmailClient.ListMessages();

                foreach (GmailMessageInfo info in messageInfos)
                {
                    // Fetch the full message to access its properties (e.g., Subject)
                    using (MailMessage fullMessage = gmailClient.FetchMessage(info.Id))
                    {
                        // Apply filter: keep only messages whose subject contains "Invoice"
                        if (!string.IsNullOrEmpty(fullMessage.Subject) &&
                            fullMessage.Subject.Contains("Invoice", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Subject: {fullMessage.Subject}");
                            Console.WriteLine($"From: {fullMessage.From}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine(ex.Message);
        }
    }
}
