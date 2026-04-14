using System;
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
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.From.Contains("@example.com");
                builder.InternalDate.Since(DateTime.UtcNow.AddDays(-7));
                MailQuery query = builder.GetQuery();

                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);
                foreach (ExchangeMessageInfo message in messages)
                {
                    Console.WriteLine(message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
