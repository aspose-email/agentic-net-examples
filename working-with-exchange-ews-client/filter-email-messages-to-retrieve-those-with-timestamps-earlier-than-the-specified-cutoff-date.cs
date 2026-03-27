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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Define the cutoff date (e.g., messages before Jan 1, 2023)
                DateTime cutoffDate = new DateTime(2023, 1, 1);

                // List all messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Check the message date against the cutoff
                    if (info.Date < cutoffDate)
                    {
                        // Fetch the full message
                        using (MailMessage mail = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {mail.Subject}");
                            Console.WriteLine($"Date: {info.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
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
