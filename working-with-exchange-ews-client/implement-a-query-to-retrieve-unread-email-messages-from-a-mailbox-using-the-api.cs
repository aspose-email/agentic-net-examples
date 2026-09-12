using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Mailbox connection parameters
                string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Build a query to fetch only unread messages
                    MailQuery unreadQuery = new MailQuery("IsRead eq false");

                    // Retrieve unread messages from the Inbox folder
                    ExchangeMessageInfoCollection unreadMessages = client.ListMessages("Inbox", unreadQuery);

                    // Output basic information about each unread message
                    foreach (ExchangeMessageInfo messageInfo in unreadMessages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                        Console.WriteLine($"Received: {messageInfo.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Gracefully report any errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
