using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                // Mark each message as read to avoid duplicate processing
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    // UniqueUri uniquely identifies the message on the server
                    client.SetReadFlag(messageInfo.UniqueUri);
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream
            Console.Error.WriteLine(ex.Message);
        }
    }
}
