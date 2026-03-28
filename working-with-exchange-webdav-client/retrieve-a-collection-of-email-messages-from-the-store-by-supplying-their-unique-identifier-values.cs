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
            // Initialize Gmail client with placeholder credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // Retrieve list of message infos (contains IDs)
                List<GmailMessageInfo> messageInfos = gmailClient.ListMessages();

                // Collect message IDs
                List<string> ids = new List<string>();
                foreach (GmailMessageInfo info in messageInfos)
                {
                    if (!string.IsNullOrEmpty(info.Id))
                    {
                        ids.Add(info.Id);
                    }
                }

                // Fetch each message by its ID and display the subject
                foreach (string id in ids)
                {
                    using (MailMessage message = gmailClient.FetchMessage(id))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
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
