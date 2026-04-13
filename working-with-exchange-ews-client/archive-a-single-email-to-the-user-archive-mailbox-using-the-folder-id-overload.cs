using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URI and credentials)
            string ewsUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (ewsUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(ewsUri, username, password))
            {
                // Source folder URI (e.g., Inbox)
                string sourceFolderUri = client.MailboxInfo.InboxUri;

                // Retrieve the first message from the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri, 1);
                if (messages != null && messages.Count > 0)
                {
                    ExchangeMessageInfo messageInfo = messages[0];

                    // Archive the message using its unique identifier
                    client.ArchiveItem(sourceFolderUri, messageInfo.UniqueUri);
                    Console.WriteLine("Message archived successfully.");
                }
                else
                {
                    Console.WriteLine("No messages found to archive.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
