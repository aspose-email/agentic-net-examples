using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);

                    // List messages in the inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    foreach (var msgInfo in messages)
                    {
                        Console.WriteLine("Subject: " + msgInfo.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
