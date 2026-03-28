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
                    // Retrieve list of message metadata
                    List<GmailMessageInfo> messagesInfo = gmailClient.ListMessages();

                    foreach (GmailMessageInfo info in messagesInfo)
                    {
                        // Fetch full message to access subject and body
                        using (MailMessage message = gmailClient.FetchMessage(info.Id))
                        {
                            // Case‑sensitive filter: subject must contain the exact word "Invoice"
                            if (message.Subject != null && message.Subject.Contains("Invoice"))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Date: {message.Date}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Gmail operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
