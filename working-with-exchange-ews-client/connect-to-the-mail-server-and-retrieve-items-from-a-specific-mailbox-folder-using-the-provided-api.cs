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
            // Server connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // Specify the folder to read (e.g., Inbox)
                string folderUri = mailboxInfo.InboxUri;

                // List messages in the specified folder
                ExchangeMessageInfoCollection messages = client.ListMessages(folderUri);

                // Iterate through the messages and display their subjects
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message using its unique URI
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
