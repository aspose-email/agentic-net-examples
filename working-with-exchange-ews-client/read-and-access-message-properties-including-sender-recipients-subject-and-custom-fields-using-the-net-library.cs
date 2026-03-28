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
            // Initialize EWS client with placeholder credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Fetch the first message
                ExchangeMessageInfo firstInfo = messages[0];
                MailMessage mail = client.FetchMessage(firstInfo.UniqueUri);

                // Access standard properties
                Console.WriteLine("Subject: " + mail.Subject);
                Console.WriteLine("From: " + mail.From);
                Console.WriteLine("To: " + string.Join(", ", mail.To));
                Console.WriteLine("Date: " + mail.Date);

                // Access custom header fields safely (HeaderCollection does not have ContainsKey)
                string customHeaderName = "X-Custom-Field";
                string customHeaderValue = mail.Headers[customHeaderName];
                if (!string.IsNullOrEmpty(customHeaderValue))
                {
                    Console.WriteLine($"{customHeaderName}: {customHeaderValue}");
                }
                else
                {
                    Console.WriteLine($"{customHeaderName} not present.");
                }

                // Dispose the fetched MailMessage
                mail.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
