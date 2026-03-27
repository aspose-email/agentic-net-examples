using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox service URL and impersonated user credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("impersonatedUser", "password", "DOMAIN");

            // Create EWS client using impersonated credentials
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a query to find messages with a specific subject keyword
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Test");
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message for each result
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + message.From);
                    }
                }

                // Send a new email as the impersonated user
                using (MailMessage newMessage = new MailMessage())
                {
                    newMessage.From = new MailAddress("impersonatedUser@example.com");
                    newMessage.To.Add("recipient@example.com");
                    newMessage.Subject = "Impersonation Test";
                    newMessage.Body = "This email was sent using impersonated credentials.";

                    client.Send(newMessage);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
