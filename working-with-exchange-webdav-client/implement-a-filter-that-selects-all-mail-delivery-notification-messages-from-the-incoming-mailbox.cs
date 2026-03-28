using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with placeholder credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId", "clientSecret", "refreshToken", "user@example.com"))
            {
                // Retrieve list of messages from the inbox
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                foreach (GmailMessageInfo info in messages)
                {
                    // Fetch the full message to examine its subject
                    using (MailMessage message = gmailClient.FetchMessage(info.Id))
                    {
                        if (message?.Subject != null &&
                            message.Subject.IndexOf("delivery", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine($"Delivery notification: {message.Subject}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
