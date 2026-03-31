using Aspose.Email.Tools.Search;
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
            // Placeholder connection data – replace with real values when running against a real server.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data.
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping EWS call.");
                return;
            }

            // Create the EWS client using the recommended factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Build an Advanced Query Syntax (AQS) expression.
                // AQS allows expressive searches such as:
                //   from:"john@example.com" subject:"report" hasattachment:true
                // The builder provides strongly‑typed access to each field.
                ExchangeAdvancedSyntaxQueryBuilder aqsBuilder = new ExchangeAdvancedSyntaxQueryBuilder();
                aqsBuilder.From.Contains("john@example.com");
                aqsBuilder.Subject.Contains("report");
                aqsBuilder.HasAttachment.Equals(true);

                // Convert the builder configuration into a MailQuery object.
                MailQuery aqsQuery = aqsBuilder.GetQuery();

                // List messages from the Inbox that match the AQS query.
                // The ListMessages overload accepts a MailQuery (including AQS queries) and a recursion flag.
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    aqsQuery);

                // Output basic information about the matched messages.
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Has Attachment: {info.HasAttachments}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
