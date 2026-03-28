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
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                try
                {
                    // Retrieve list of message infos
                    List<GmailMessageInfo> messageInfos = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messageInfos)
                    {
                        // Fetch the full message to access its plain‑text body
                        MailMessage fullMessage = gmailClient.FetchMessage(info.Id);
                        string subject = fullMessage.Subject ?? string.Empty;
                        string body = fullMessage.Body ?? string.Empty;

                        Console.WriteLine("Subject: {0}", subject);
                        Console.WriteLine("Body: {0}", body);
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while listing or fetching messages: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
