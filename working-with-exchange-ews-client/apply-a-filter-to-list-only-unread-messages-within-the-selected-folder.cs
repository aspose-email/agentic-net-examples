using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange Web Services URL and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Folder to search – using the standard Inbox name
                string inboxFolder = "Inbox";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Retrieve all messages in the folder
                ExchangeMessageInfoCollection allMessages = client.ListMessages(inboxFolder);

                Console.WriteLine("Unread messages in the Inbox:");
                foreach (ExchangeMessageInfo messageInfo in allMessages)
                {
                    // Filter only unread messages
                    if (!messageInfo.IsRead)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
