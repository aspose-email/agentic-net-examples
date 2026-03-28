using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Designated sender to filter messages from
                string designatedSender = "sender@example.com";

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages("Inbox");

                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Fetch the full message using its unique URI
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        if (message.From != null &&
                            string.Equals(message.From.Address, designatedSender, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
