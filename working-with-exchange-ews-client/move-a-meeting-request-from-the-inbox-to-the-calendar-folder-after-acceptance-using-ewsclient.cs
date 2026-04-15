using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and user credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Get the URI of the Inbox folder
                string inboxFolderUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(inboxFolderUri);

                foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                {
                    // Identify a meeting request (simple subject check)
                    if (messageInfo.Subject != null && messageInfo.Subject.Contains("Meeting Request"))
                    {
                        // Get the Calendar folder URI
                        string calendarFolderUri = client.MailboxInfo.CalendarUri;

                        // Move the meeting request to the Calendar folder
                        client.MoveItem(messageInfo.UniqueUri, calendarFolderUri);

                        Console.WriteLine("Meeting request moved to Calendar folder.");
                        break;
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
