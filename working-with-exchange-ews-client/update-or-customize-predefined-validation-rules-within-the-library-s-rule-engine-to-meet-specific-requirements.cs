using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // List messages from the Inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Output basic information about each message.
                    Console.WriteLine("Subject: " + info.Subject);
                    // Use the correct property for the message date.
                    Console.WriteLine("Date: " + info.Date);
                }
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error output.
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
